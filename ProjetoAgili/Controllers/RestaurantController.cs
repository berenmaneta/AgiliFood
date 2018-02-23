using ProjetoAgili.App_Start;
using ProjetoAgili.Commom;
using ProjetoAgili.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProjetoAgili.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly AgiliFoodContainer db = new AgiliFoodContainer();

        #region GET

        public ActionResult Dashboard()
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("3"))
            {
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AllowAnonymous]
        public ActionResult MyAccount()
        {
            if (Session["UserID"] != null)
            {
                int id = Convert.ToInt32(Session["UserID"]);
                ResetPasswordViewModel model = new ResetPasswordViewModel();
                model.Email = Session["UserName"].ToString();
                model.Password = "";
                model.ConfirmPassword = "";
                var rest = db.Restaurants.Where(x => x.IdUser == id).FirstOrDefault();
                model.Name = rest.Name;
                model.StreetAddress = rest.StreetAddress;
                model.Phone = rest.Phone;
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }



        #endregion

        #region GET ADD

        public ActionResult AddIngredient()
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("3"))
            {
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        public ActionResult AddDishes()
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("3"))
            {
                DishesModel model = new DishesModel();

                ViewBag.Categories = from Enums.Categories o in Enum.GetValues(typeof(Enums.Categories))
                                     select new { ID = (int)o, Name = o.ToString() };

                ViewBag.Options = from Enums.Generos o in Enum.GetValues(typeof(Enums.Generos))
                                  select new { IsOnMenu = (int)o, Name = o.ToString() };

                var userID = Convert.ToInt32(Session["UserID"]);

                model.IdRestaurant = db.Users.Where(x => x.IdUser == userID).FirstOrDefault().Restaurants.FirstOrDefault().IdRestaurant;

                return View(model);
            }
            return RedirectToAction("Login", "Home");
        }

        #endregion

        #region GET SHOW

        public ActionResult ShowIngredients()
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("3"))
            {
                int id = Convert.ToInt32(Session["UserID"]);
                int idrest = db.Restaurants.Where(x => x.IdUser == id).FirstOrDefault().IdRestaurant;
                ShowIngredientsModel model = new ShowIngredientsModel();
                model.Ingredients = new List<IngredientesModel>();
                var lista = db.Ingredients.Where(x => x.IdRestaurant == idrest).ToList();

                foreach (var ing in lista)
                {
                    IngredientesModel mod = new IngredientesModel();
                    mod.IdIngrediente = ing.IdIngredient;
                    mod.IdRestaurant = ing.IdRestaurant;
                    mod.Name = ing.Name;
                    model.Ingredients.Add(mod);
                }
                return View(model);
            }
            return RedirectToAction("Login", "Home");
        }

        public ActionResult ShowDishes()
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("3"))
            {
                var userID = Convert.ToInt32(Session["UserID"]);
                ShowDishesModel model = new ShowDishesModel();
                model.Dishes = new List<DishesModel>();

                var lista = db.Dishes.Where(x => x.Restaurants.IdUser == userID).ToList();

                foreach (var dish in lista)
                {
                    DishesModel mod = new DishesModel();
                    mod.Description = dish.Description;
                    mod.Category = dish.Category.Name;
                    mod.IdDish = dish.IdDish;
                    mod.IdRestaurant = dish.IdRestaurant;
                    mod.IsOnMenu = dish.IsOnMenu == true ? 1 : 2;
                    mod.Name = dish.Name;
                    mod.Price = dish.Price;
                    mod.IdUser = dish.Restaurants.IdUser;
                    model.Dishes.Add(mod);
                }

                return View(model);

            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult ShowReports()
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("3"))
            {
                var userID = Convert.ToInt32(Session["UserID"]);
                ShowReportViewModel model = new ShowReportViewModel();
                model.Reports = new List<ReportModel>();

                var lista = db.Pedido.Where(x => x.Dishes.Restaurants.IdUser == userID).ToList();

                foreach (var ped in lista)
                {
                    ReportModel mod = new ReportModel();

                    mod.IdUser = ped.IdUser;
                    var pessoa = db.Pessoa.Where(x => x.IdUser == ped.IdUser).FirstOrDefault();
                    if (pessoa != null)
                        mod.Name = pessoa.Name;
                    else
                        mod.Name = ped.Users.Email;
                    mod.Total = db.Pedido.Where(x => x.IdUser == ped.IdUser).ToList().Sum(x => x.Price);
                    model.Reports.Add(mod);
                    model.Total += mod.Total;
                }

                return View(model);

            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }


        #endregion

        #region GET EDIT

        public ActionResult EditDish(int id)
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("3"))
            {
                var userId = Convert.ToInt32(Session["UserID"]);
                DishesModel model = new DishesModel();

                ViewBag.Categories = from Enums.Categories o in Enum.GetValues(typeof(Enums.Categories))
                                     select new { ID = (int)o, Name = o.ToString() };
                ViewBag.Options = from Enums.Generos o in Enum.GetValues(typeof(Enums.Generos))
                                  select new { IsOnMenu = (int)o, Name = o.ToString() };

                var dish = db.Dishes.Where(x => x.IdDish == id).FirstOrDefault();

                model.Name = dish.Name;
                model.IdCategory = (int)dish.IdCategory;
                model.Description = dish.Description;
                model.IdDish = dish.IdDish;
                model.IdRestaurant = dish.IdRestaurant;
                model.IsOnMenu = dish.IsOnMenu == true ? 1 : 2;
                model.Price = dish.Price;

                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult EditIngredient(int id)
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("3"))
            {
                IngredientesModel model = new IngredientesModel();
                var ing = db.Ingredients.Where(x => x.IdIngredient == id).FirstOrDefault();

                model.IdIngrediente = ing.IdIngredient;
                model.IdRestaurant = ing.IdRestaurant;
                model.Name = ing.Name;

                return View(model);
            }
            return RedirectToAction("Login", "Home");
        }

        #endregion

        #region POST EDIT

        [HttpPost]
        public ActionResult EditIngredient(IngredientesModel model)
        {
            var ing = db.Ingredients.Where(x => x.IdIngredient == model.IdIngrediente).FirstOrDefault();
            ing.Name = model.Name;

            db.Entry(ing).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ShowIngredients", "Restaurant");
        }

        [HttpPost]
        public ActionResult EditDish(HttpPostedFileBase file, DishesModel model)
        {
            if (file != null)
                model.Photo = Image.ConvertToBytes(file);

            var dish = db.Dishes.Where(x => x.IdDish == model.IdDish).FirstOrDefault();

            dish.Name = model.Name;
            dish.Description = model.Description;
            dish.Photo = model.Photo;
            dish.Price = model.Price;
            dish.IsOnMenu = model.IsOnMenu == 1 ? true : false;
            dish.IdRestaurant = model.IdRestaurant;
            dish.IdCategory = model.IdCategory;

            db.Entry(dish).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ShowDishes", "Restaurant");
        }

        #endregion

        #region POST ADD

        [HttpPost]
        public ActionResult AddIngredient(DishesModel model)
        {
            var userId = Convert.ToInt32(Session["UserID"]);
            if (Session["UserID"] != null && Session["Profile"].Equals("3"))
            {
                var Ingredient = new Ingredients
                {
                    Name = model.Name,
                    IdRestaurant = db.Restaurants.Where(x => x.IdUser == userId).FirstOrDefault().IdRestaurant
                };
                db.Ingredients.Add(Ingredient);
                int i = db.SaveChanges();
                if (i == 1)
                {
                    return RedirectToAction("ShowIngredients", "Restaurant");
                }
                else
                {
                    return RedirectToAction("Dashboard", "Restaurant");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public ActionResult AddDish(HttpPostedFileBase file, DishesModel model)
        {
            var userId = Convert.ToInt32(Session["UserID"]);

            if(file != null)
                model.Photo = Image.ConvertToBytes(file);
            var Dishes = new Dishes
            {
                Name = model.Name,
                Description = model.Description,
                Photo = model.Photo,
                Price = model.Price,
                IsOnMenu = model.IsOnMenu == 1 ? true : false,
                IdRestaurant = model.IdRestaurant,
                IdCategory = model.IdCategory
            };
            db.Dishes.Add(Dishes);
            int i = db.SaveChanges();
            if (i == 1)
            {
                return RedirectToAction("ShowDishes", "Restaurant");
            }
            else
            {
                return RedirectToAction("Dashboard", "Restaurant");
            }
        }
        #endregion

        #region INSERIR        

        public ActionResult InserirIngrediente()
        {
            if (Session["UserID"] != null)
            {
                var ingredienteDish = new IngredientOnAPlate();

                var userId = Convert.ToInt32(Session["UserID"]);
                var idRest = db.Restaurants.Where(x => x.IdUser == userId).FirstOrDefault().IdRestaurant;
                var ingredients = db.Ingredients.Where(x => x.IdRestaurant == idRest).ToList();
                var dishes = db.Dishes.Where(x => x.IdRestaurant == idRest).ToList();

                ViewBag.numIngredients = ingredients.Count;
                ViewBag.numDishes = dishes.Count;

                IngredienteDishViewModel model = new IngredienteDishViewModel(ingredienteDish, ingredients, dishes);

                ViewBag.ingredientDish = ingredienteDish;
                return View(model);

            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InserirIngrediente(IngredienteDishViewModel ingredientDish)
        {
            if (ModelState.IsValid)
            {
                foreach (var aa in ingredientDish.SelectedIngredients)
                {
                    var userId = Convert.ToInt32(Session["UserID"]);
                    var teste = db.IngredientOnAPlate.Where(x => x.IdIngredient == aa && x.IdDish == ingredientDish.IdDish).ToList();
                    if (teste.Count == 0)
                    {
                        var ingDish = db.Set<IngredientOnAPlate>();
                        ingDish.Add(new IngredientOnAPlate { IdIngredient = aa, IdDish = ingredientDish.IdDish });
                        db.SaveChanges();
                    }
                }          
            }
            return RedirectToAction("ShowDishes");
        }

        #endregion

        #region POST DELETE

        [HttpPost]
        public ActionResult DeleteDish(int id)
        {
            if (ModelState.IsValid)
            {
                int userID = Convert.ToInt32(Session["UserID"]);

                if (id > 0)
                {
                    var idRest = db.Restaurants.Where(x => x.IdUser == userID).FirstOrDefault().IdRestaurant;

                    var dish = db.Dishes.Where(x => x.IdDish == id && x.IdRestaurant == idRest).FirstOrDefault();

                    var pedidos = db.Pedido.Where(x => x.IdDish == dish.IdDish).ToList();

                    foreach(var ped in pedidos)
                    {
                        db.Entry(ped).State = System.Data.Entity.EntityState.Deleted;
                    }

                    var onAPlate = db.IngredientOnAPlate.Where(x => x.IdDish == dish.IdDish).ToList();

                    foreach (var on in onAPlate)
                    {
                        db.Entry(on).State = System.Data.Entity.EntityState.Deleted;
                    }

                    db.Entry(dish).State = System.Data.Entity.EntityState.Deleted;

                    db.SaveChanges();
                }
                
            }
            return RedirectToAction("ShowDishes");
        }

        [HttpPost]
        public ActionResult DeleteIngredient(int id)
        {
            if (ModelState.IsValid)
            {
                int userID = Convert.ToInt32(Session["UserID"]);

                if (id > 0)
                {
                    var idRest = db.Restaurants.Where(x => x.IdUser == userID).FirstOrDefault().IdRestaurant;

                    var ingredient = db.Ingredients.Where(x => x.IdIngredient == id && x.IdRestaurant == idRest).FirstOrDefault();

                    var onAPlate = db.IngredientOnAPlate.Where(x => x.IdIngredient == ingredient.IdIngredient).ToList();

                    foreach (var on in onAPlate)
                    {
                        db.Entry(on).State = System.Data.Entity.EntityState.Deleted;
                    }

                    db.Entry(ingredient).State = System.Data.Entity.EntityState.Deleted;

                    db.SaveChanges();
                }

            }
            return RedirectToAction("ShowIngredients");
        }

        #endregion

        #region HELPERS

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Dashboard", "Restaurant");
            }
            var userId = Convert.ToInt32(Session["UserID"]);

            DataChange.ResetPassword(userId, db, model);

            return RedirectToAction("Dashboard", "Restaurant");
        }

        [HttpPost]
        public ActionResult DataProfile(DataProfileModel model)
        {
            if (ModelState.IsValid)
            {
                var id = Convert.ToInt32(Session["UserID"]);
                var profile = Convert.ToInt32(Session["Profile"]);
                DataChange.DataProfile(model, id, profile, db);
            }
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file)
        {
            var id = Convert.ToInt32(Session["UserID"]);
            var profile = Convert.ToInt32(Session["Profile"]);
            Image.UploadImage(file, profile, id, db);
            return RedirectToAction("Dashboard", "Restaurant");
        }

        #endregion
    }
}