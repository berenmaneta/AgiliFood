using ProjetoAgili.App_Start;
using ProjetoAgili.Commom;
using ProjetoAgili.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ProjetoAgili.Controllers
{
    public class AdministratorsController : Controller
    {
        private readonly AgiliFoodContainer db = new AgiliFoodContainer();

        #region GET

        public ActionResult Dashboard()
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("2"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }          
        }

        public ActionResult Reports()
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("2"))
            {
                ShowOrdersModel model = new ShowOrdersModel();
                model.lista = new List<OrdersModel>();

                ViewBag.Meses = from Enums.Meses o in Enum.GetValues(typeof(Enums.Meses))
                                select new { ID = (int)o, Name = o.ToString() };

                int mes = DateTime.Now.Month;

                var lista = db.Users.ToList();
                foreach (var ped in lista)
                { 
                    if(ped.Pedido.Count > 0)
                    {
                        OrdersModel mod = new OrdersModel();

                        mod.IdUser = ped.IdUser;
                        mod.Price = ped.Pedido.Where(x => x.Date.Month == mes).Sum(x => x.Price);
                        mod.UserName = ped.Email;

                        model.Total += mod.Price;
                        model.lista.Add(mod);
                    }                    
                }

                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public ActionResult ReportsByMonth(ShowOrdersModel model)
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("2"))
            {
                model.lista = new List<OrdersModel>();

                ViewBag.Meses = from Enums.Meses o in Enum.GetValues(typeof(Enums.Meses))
                                select new { ID = (int)o, Name = o.ToString() };

                var lista = db.Users.ToList();
                foreach (var ped in lista)
                {
                    if (ped.Pedido.Count > 0)
                    {
                        OrdersModel mod = new OrdersModel();

                        mod.IdUser = ped.IdUser;
                        mod.Price = ped.Pedido.Where(x => x.Date.Month == model.IdMes).Sum(x => x.Price);
                        mod.UserName = ped.Email;
                        mod.IdMes = model.IdMes;

                        model.Total += mod.Price;
                        model.lista.Add(mod);
                  
                    }
                }

                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult ReportsByUser(int id, int mes)
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("2"))
            {
                ShowOrdersModel model = new ShowOrdersModel();
                model.lista = new List<OrdersModel>();

                var lista = new List<Pedido>();
                var pedidos = db.Pedido.Where(x => x.IdUser == id).ToList();
                if(mes > 0)
                {
                    lista = pedidos.Where(x => x.Date.Month == mes).ToList();
                }
                else
                {
                    lista = pedidos;
                }
                foreach (var ped in lista)
                {
                    OrdersModel mod = new OrdersModel();

                    mod.IdUser = ped.IdUser;
                    mod.Price = ped.Price;
                    mod.UserName = ped.Users.Email;
                    mod.Date = ped.Date;
                    mod.DishName = ped.Dishes.Name;
                    mod.RestaurantName = ped.Dishes.Restaurants.Name;
                    model.IdMes = mes;
                    model.Mes = ((Enums.Meses)mes).ToString();
                    model.Total += mod.Price;
                    model.lista.Add(mod);
                    
                }

                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }



        [AllowAnonymous]
        public ActionResult MyAccount()
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("2"))
            {
                int id = Convert.ToInt32(Session["UserID"]);
                ResetPasswordViewModel model = new ResetPasswordViewModel();
                model.Email = Session["UserName"].ToString();
                model.Password = "";
                model.ConfirmPassword = "";
                var admin = db.Administrators.Where(x => x.IdUser == id).FirstOrDefault();
                model.Name = admin.Name;
                model.Phone = admin.Phone;
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        #endregion

        #region GET SHOW


        //MÉTODO SHOWUSERS
        //RETORNA A VIEW COM O MODEL POPULADO COM 
        //OS USUÁRIOS CADASTRADOS NO SISTEMA
        public ActionResult ShowUsers()
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("2"))
            {
                ShowUsersModel model = new ShowUsersModel();
                model.Users = new List<UsersModel>();
                var obj = db.Users.Where(x => x.Profile == 1);

                foreach (var user in obj)
                {
                    UsersModel mod = new UsersModel();
                    mod.Name = db.Pessoa.Where(x => x.IdUser == user.IdUser).FirstOrDefault().Name;
                    mod.Email = user.Email;
                    mod.IdUsuario = user.IdUser;
                    model.Users.Add(mod);
                }

                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        //MÉTODO SHOWRESTAURANTS
        //RETORNA A VIEW COM O MODEL POPULADO COM 
        //OS RESTAURANTES CADASTRADOS NO SISTEMA
        public ActionResult ShowRestaurants(bool enabled)
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("2"))
            {
                ShowRestaurantsModel model = new ShowRestaurantsModel();
                model.Restaurants = new List<RestaurantsModel>();
                var obj = db.Restaurants.Where(x => x.Enabled == enabled);

                foreach (var rest in obj)
                {
                    RestaurantsModel mod = new RestaurantsModel();
                    mod.Name = rest.Name;
                    mod.IdUser = rest.IdUser;
                    mod.IdRestaurant = rest.IdRestaurant;
                    mod.Phone = rest.Phone;
                    mod.StreetAddress = rest.StreetAddress;
                    mod.Enabled = rest.Enabled == true ? 1 : 2;

                    model.Restaurants.Add(mod);
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

        //MÉTODO EDITUSER
        //RETORNA A VIEW COM O MODEL POPULADO COM 
        //OS USUÁRIOS CADASTRADOS NO SISTEMA PARA EDIÇÃO
        public ActionResult EditUser(int id)
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("2"))
            {
                UsersModel model = new UsersModel();
                var obj = db.Users.Where(x => x.IdUser == id).ToList().FirstOrDefault();

                model.IdProfile = obj.Profile;
                model.Email = obj.Email;
                model.IdUsuario = obj.IdUser;
                model.Password = obj.Password;
                model.PasswordCheck = obj.Password;
                model.Name = db.Pessoa.Where(x => x.IdUser == obj.IdUser).FirstOrDefault().Name;

                ViewBag.Profiles = db.Profile.ToList();

                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

        }

        //MÉTODO EDITRESTAURANT
        //RETORNA A VIEW COM O MODEL POPULADO COM 
        //OS RESTAURANTES CADASTRADOS NO SISTEMA PARA EDIÇÃO
        public ActionResult EditRestaurant(int id)
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("2"))
            {
                ViewBag.Options = from Enums.Generos o in Enum.GetValues(typeof(Enums.Generos))
                                  select new { Enabled = (int)o, Name = o.ToString() };

                RestaurantsModel model = new RestaurantsModel();
                var obj = db.Restaurants.Where(x => x.IdRestaurant == id).ToList().FirstOrDefault();

                model.Name = obj.Name;
                model.IdRestaurant = obj.IdRestaurant;
                model.IdUser = obj.IdUser;
                model.Phone = obj.Phone;
                model.StreetAddress = obj.StreetAddress;
                model.Enabled = obj.Enabled == true ? 1 : 2;
                model.Email = db.Users.Where(x => x.IdUser == obj.IdUser).FirstOrDefault().Email;

                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

        }

        #endregion

        #region GET ADD

        public ActionResult AddUser()
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("2"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult AddRestaurant()
        {
            if (Session["UserID"] != null && Session["Profile"].Equals("2"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        #endregion

        #region POST EDIT

        //MÉTODO EDITUSER
        //ENTRADA: USERSMODEL
        //RECUPERA AS INFORMAÇÕES DO MODEL E
        //ATUALIZA O REGISTRO NO BANCO DE DADOS
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(UsersModel model)
        {
            if (ModelState.IsValid)
            {
                Users usuario = new Users();

                if (model != null)
                    usuario = db.Users.Where(x => x.IdUser == model.IdUsuario).FirstOrDefault();
 
                usuario.Email = model.Email;

                db.Entry(usuario).State = System.Data.Entity.EntityState.Modified;

                Pessoa pessoa = new Pessoa();
                pessoa = db.Pessoa.Where(x => x.IdUser == model.IdUsuario).FirstOrDefault();
                if(pessoa != null)
                    pessoa.Name = model.Name;
                db.Entry(usuario).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();          
            }

            return RedirectToAction("ShowUsers");
        }

        //MÉTODO EDITRESTAURANT
        //ENTRADA: RESTAURANTMODEL
        //RECUPERA AS INFORMAÇÕES DO MODEL E
        //ATUALIZA O REGISTRO NO BANCO DE DADOS
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRestaurant(RestaurantsModel model)
        {
            if (ModelState.IsValid)
            {
                Restaurants rest = new Restaurants();

                if (model != null)
                    rest = db.Restaurants.Where(x => x.IdUser == model.IdUser).FirstOrDefault();

                rest.Name = model.Name;
                rest.Phone = model.Phone;
                rest.StreetAddress = model.StreetAddress;
                rest.Enabled = model.Enabled == 1 ? true : false;

                db.Entry(rest).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            if(model.Enabled == 1)
                return RedirectToAction("ShowRestaurants", new { enabled = true });
            else
                return RedirectToAction("ShowRestaurants", new { enabled = false });
        }

        #endregion

        #region POST ADD

        //MÉTODO ADDUSER
        //ENTRADA: ADDUSERSMODEL
        //RECUPERA AS INFORMAÇÕES DO MODEL E
        //INSERE UM NOVO REGISTRO NO BANCO DE DADOS
        [HttpPost]
        public ActionResult AddUser(AddUserModel model)
        {
            if (ModelState.IsValid)
            {
                var senha = Hash.MD5Hash("123");
                var userId = Convert.ToInt32(Session["UserID"]);
                var usuario = db.Users.Where(x => x.Email == model.Email && x.Password == senha).ToList().FirstOrDefault();
                if (usuario == null)
                {
                    var item = db.Set<Users>();
                    item.Add(new Users
                    {
                        Email = model.Email,
                        Password = senha,
                        Profile = 1
                    });

                    db.SaveChanges();

                    var id = db.Users.Where(x => x.Email.Equals(model.Email)).FirstOrDefault().IdUser;

                    var pessoa = db.Set<Pessoa>();
                    pessoa.Add(new Pessoa
                    {
                        Name = model.Name,
                        IdUser = id
                    });

                    db.SaveChanges();
                }

            }
            return RedirectToAction("ShowUsers", "Administrators");
        }

        //MÉTODO ADDRESTAURANT
        //ENTRADA: ADDRESTAURANTMODEL
        //RECUPERA AS INFORMAÇÕES DO MODEL E
        //INSERE UM NOVO REGISTRO NO BANCO DE DADOS
        [HttpPost]
        public ActionResult AddRestaurant(AddRestaurantModel model)
        {
            if (ModelState.IsValid)
            {
                var senha = Hash.MD5Hash("123");
                var userId = Convert.ToInt32(Session["UserID"]);
                var usuario = db.Users.Where(x => x.Email == model.Email && x.Password == senha).ToList().FirstOrDefault();
                if (usuario == null)
                {
                    var item = db.Set<Users>();
                    item.Add(new Users
                    {
                        Email = model.Email,
                        Password = senha,
                        Profile = 3
                    });

                    db.SaveChanges();

                    var id = db.Users.Where(x => x.Email.Equals(model.Email)).FirstOrDefault().IdUser;

                    var pessoa = db.Set<Restaurants>();
                    pessoa.Add(new Restaurants
                    {
                        Name = model.Name,
                        IdUser = id,
                        StreetAddress = model.StreetAddress,
                        Phone = model.Phone,
                        Enabled = true
                    });

                    db.SaveChanges();
                }

            }
            return RedirectToAction("ShowRestaurants", "Administrators", new { enabled = true });
        }

        #endregion

        #region POST DELETE

        //MÉTODO DELETEUSER
        //ENTRADA: INT ID DO REGISTRO A SER DELETADO
        //EXCLUI O REGISTRO DO BANCO DE DADOS
        [HttpPost]
        public ActionResult DeleteUser(int id)
        {
            if (ModelState.IsValid)
            {
                if (id > 0)
                {
                    var user = db.Users.Where(x => x.IdUser == id).FirstOrDefault();
                    var pessoa = db.Pessoa.Where(x => x.IdUser == id).FirstOrDefault();

                    var pedidos = db.Pedido.Where(x => x.IdUser == user.IdUser).ToList();
                    foreach (var ped in pedidos)
                    {
                        db.Entry(ped).State = System.Data.Entity.EntityState.Deleted;
                    }

                    if (pessoa != null)
                        db.Entry(pessoa).State = System.Data.Entity.EntityState.Deleted;
                    db.Entry(user).State = System.Data.Entity.EntityState.Deleted;

                    db.SaveChanges();
                }

            }
            return RedirectToAction("ShowUsers");
        }

        //MÉTODO DELETERESTAURANT
        //ENTRADA: INT ID DO REGISTRO A SER DELETADO
        //EXCLUI O REGISTRO DO BANCO DE DADOS
        [HttpPost]
        public ActionResult DeleteRestaurant(int id)
        {
            if (ModelState.IsValid)
            {
                if (id > 0)
                {
                    var rest = db.Restaurants.Where(x => x.IdRestaurant == id).FirstOrDefault();
                    var user = db.Users.Where(x => x.IdUser == rest.IdUser).FirstOrDefault();

                    var pedidos = db.Pedido.Where(x => x.Dishes.IdRestaurant == rest.IdRestaurant).ToList();
                    foreach (var p in pedidos)
                    {
                        db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
                    }

                    var onAPlate = db.IngredientOnAPlate.Where(x => x.Dishes.IdRestaurant == rest.IdRestaurant).ToList();
                    foreach (var op in onAPlate)
                    {
                        db.Entry(op).State = System.Data.Entity.EntityState.Deleted;
                    }

                    var dishes = db.Dishes.Where(x => x.IdRestaurant == rest.IdRestaurant).ToList();
                    foreach (var d in dishes)
                    {
                        db.Entry(d).State = System.Data.Entity.EntityState.Deleted;
                    }

                    var ingredientes = db.Ingredients.Where(x => x.IdRestaurant == rest.IdRestaurant).ToList();
                    foreach (var i in ingredientes)
                    {
                        db.Entry(i).State = System.Data.Entity.EntityState.Deleted;
                    }

                    db.Entry(rest).State = System.Data.Entity.EntityState.Deleted;
                    db.Entry(user).State = System.Data.Entity.EntityState.Deleted;

                    db.SaveChanges();
                }

            }
            return RedirectToAction("ShowRestaurants");
        }

        #endregion

        #region HELPERS

        //MÉTODO RESETPASSWORD
        //ENTRADA: CHANGEPASSWORDMODEL
        //CHAMA O MÉTODO RESETPASSWORD DA CLASSE DATACHANGE
        //QUE MODIFICA A SENHA NO BANCO DE DADOS
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Dashboard", "Administrators");
            }
            var userId = Convert.ToInt32(Session["UserID"]);

            DataChange.ResetPassword(userId, db, model);

            return RedirectToAction("Dashboard", "Administrators");
        }

        //MÉTODO UPLOADIMAGE
        //ENTRADA: HTTPPOSTEDFILEBASE
        //RECUPERA UM ARQUIVO DE IMAGEM DA VIEW E CHAMA O MÉTODO
        //UPLOADIMAGE DA CLASSE IMAGE PARA INSERIR O ARQUIVO 
        //NO BANCO DE DADOS
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file)
        {
            var id = Convert.ToInt32(Session["UserID"]);
            var profile = Convert.ToInt32(Session["Profile"]);
            Image.UploadImage(file, profile, id, db);
            return RedirectToAction("Dashboard", "Administrators");
        }

        //MÉTODO DATAPROFILE
        //ENTRADA: DATAPROFILEMODEL
        //RECUPERA OS DADOS DO MODEL E CHAMA O MÉTODO DATAPROFILE
        //DA CLASSE DATACHANGE QUE ATUALIZA O REGISTRO
        //NO BANCO DE DADOS
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

        #endregion


    }
}