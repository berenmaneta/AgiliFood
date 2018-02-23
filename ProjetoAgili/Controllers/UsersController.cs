using ProjetoAgili.App_Start;
using ProjetoAgili.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ProjetoAgili.Commom;

namespace ProjetoAgili.Controllers
{
    public class UsersController : Controller
    {
        private readonly AgiliFoodContainer db = new AgiliFoodContainer();

        public ActionResult Index()
        {
            if(Session["UserID"] != null && Session["Profile"].Equals("1"))
            {
                List<RestaurantsModel> model = new List<RestaurantsModel>();

                var obj = db.Restaurants.Where(x => x.Enabled == true).ToList();
                foreach(var m in obj)
                {
                    RestaurantsModel rest = new RestaurantsModel();
                    rest.Name = m.Name;
                    rest.IdUser = m.IdUser;
                    rest.Phone = m.Phone;
                    rest.StreetAddress = m.StreetAddress;
                    rest.IdRestaurant = m.IdRestaurant;

                    model.Add(rest);
                }              
                return View(model);
            }
            return RedirectToAction("Login", "Home");
        }

        public ActionResult MyAccount()
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("1"))
            {
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Users");
            }
            var userId = Convert.ToInt32(Session["UserID"]);

            DataChange.ResetPassword(userId, db, model);

            return RedirectToAction("Index", "Users");
        }

        public ActionResult ShowRestaurant(int id)
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("1"))
            {
                List<DishesModel> model = new List<DishesModel>();

                var obj = db.Dishes.Where(x => x.IdRestaurant == id).ToList();
                foreach (var m in obj)
                {
                    if (m.IsOnMenu)
                    {
                        DishesModel dish = new DishesModel();
                        dish.Name = m.Name;
                        dish.IdRestaurant = m.IdRestaurant;
                        dish.IdDish = m.IdDish;
                        dish.Price = m.Price;

                        model.Add(dish);
                    }
                }
                
                return View(model);
            }
            return RedirectToAction("Login", "Home");
        }

        public ActionResult ShowDish(int id)
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("1"))
            {
                DishesModel model = new DishesModel();
                model.Ingredients = new List<IngredientesModel>();

                var obj = db.Dishes.Where(x => x.IdDish == id).ToList().FirstOrDefault();
                if(obj != null)
                {
                    model.Name = obj.Name;
                    model.IdDish = obj.IdDish;
                    model.Description = obj.Description;
                    model.IdRestaurant = obj.IdRestaurant;
                    model.Price = obj.Price;

                    var ing = db.IngredientOnAPlate.Where(x => x.IdDish == id).ToList();

                    foreach(var ingred in ing)
                    {
                        var res = db.Ingredients.Where(x => x.IdIngredient == ingred.IdIngredient).FirstOrDefault();
                        IngredientesModel mod = new IngredientesModel();
                        mod.IdIngrediente = ingred.IdIngredient;
                        mod.Name = res.Name;
                        model.Ingredients.Add(mod);
                    }
                }
                
                return View(model);
            }
            return RedirectToAction("Login", "Home");
        }

        public ActionResult ShowCategories(int id)
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("1"))
            {
                List<DishesModel> model = new List<DishesModel>();

                var obj = db.Dishes.Where(x => x.IdCategory == id).OrderBy(x => x.IdRestaurant).ToList();
                foreach (var m in obj)
                {
                    if (m.IsOnMenu)
                    {
                        DishesModel dish = new DishesModel();
                        dish.Name = m.Name;
                        dish.IdRestaurant = m.IdRestaurant;
                        dish.IdDish = m.IdDish;
                        dish.Price = m.Price;

                        dish.RestaurantName = db.Restaurants.Where(x => x.IdRestaurant == m.IdRestaurant).FirstOrDefault().Name;

                        model.Add(dish);
                    }
                }
                
                return View(model);
            }
            return RedirectToAction("Login", "Home");
        }

        public ActionResult ConfirmOrder(int id)
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("1"))
            {
                var userId = Convert.ToInt32(Session["UserID"]);
                var preco = db.Dishes.Where(x => x.IdDish == id).FirstOrDefault().Price;

                var item = db.Set<Pedido>();
                item.Add(new Pedido { Date = DateTime.Now, IdDish = id, IdUser = userId, Price = preco });
                db.SaveChanges();
                ViewData["Message"] = "Seu pedido foi registrado!";
                
                return RedirectToAction("Index", "Users");
            }
            return RedirectToAction("Login", "Home");
        }

        public ActionResult ShowOrders()
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("1"))
            {
                ShowOrdersModel model = new ShowOrdersModel();
                model.lista = new List<OrdersModel>();

                ViewBag.Meses = from Enums.Meses o in Enum.GetValues(typeof(Enums.Meses))
                                  select new { ID = (int)o, Name = o.ToString() };

                int userId = Convert.ToInt32(Session["UserID"]);

                var lista = db.Pedido.Where(x => x.IdUser == userId).ToList();
                foreach (var ped in lista)
                {
                    OrdersModel mod = new OrdersModel();
                    mod.Date = ped.Date;
                    mod.IdDish = ped.IdDish;
                    ped.IdPedido = ped.IdPedido;
                    mod.IdUser = ped.IdUser;
                    mod.Price = ped.Price;
                    mod.DishName = ped.Dishes.Name;  //db.Dishes.Where(x => x.IdDish == ped.IdDish).FirstOrDefault().Name;
                    mod.RestaurantName = ped.Dishes.Restaurants.Name; //db.Restaurants.Where(x => x.IdRestaurant == ped.Dishes.IdRestaurant).FirstOrDefault().Name;
                    model.Total += mod.Price;

                    model.lista.Add(mod);
                }
                
                return View(model);
            }
            return RedirectToAction("Login", "Home");
            
        }

        [HttpPost]
        public ActionResult ShowOrdersByMonth(ShowOrdersModel model)
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("1"))
            {
                model.lista = new List<OrdersModel>();

                int userId = Convert.ToInt32(Session["UserID"]);

                ViewBag.Meses = from Enums.Meses o in Enum.GetValues(typeof(Enums.Meses))
                                select new { ID = (int)o, Name = o.ToString() };

                var lista = db.Pedido.Where(x => x.IdUser == userId && x.Date.Month == model.IdMes).ToList();
                foreach (var ped in lista)
                {
                    OrdersModel mod = new OrdersModel();
                    mod.Date = ped.Date;
                    mod.IdDish = ped.IdDish;
                    ped.IdPedido = ped.IdPedido;
                    mod.IdUser = ped.IdUser;
                    mod.Price = ped.Price;
                    mod.DishName = ped.Dishes.Name;  //db.Dishes.Where(x => x.IdDish == ped.IdDish).FirstOrDefault().Name;
                    mod.RestaurantName = ped.Dishes.Restaurants.Name; //db.Restaurants.Where(x => x.IdRestaurant == ped.Dishes.IdRestaurant).FirstOrDefault().Name;
                    model.Total += mod.Price;

                    model.lista.Add(mod);
                }
                
                return View(model);
            }
            return RedirectToAction("Login", "Home");
        }

        public ActionResult Checkout()
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("1"))
            {
                Session["UserID"] = null;
                Session["UserName"] = null;
                Session["Profile"] = null;
                return RedirectToAction("Login", "Home");
            }
            return RedirectToAction("Login", "Home");
        }

        public ActionResult ChangePassword()
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("1"))
            {
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        public ActionResult RetrieveImage(int id)
        {
            byte[] cover = Image.GetImageDish(id, db);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }

        public ActionResult RetrieveRestaurantImage(int id)
        {
            byte[] cover = Image.GetImageRestaurant(id, db);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }

        
    }
}