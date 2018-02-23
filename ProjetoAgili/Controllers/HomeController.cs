using ProjetoAgili.Commom;
using ProjetoAgili.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ProjetoAgili.Controllers
{
    public class HomeController : Controller
    {
        AgiliFoodContainer db = new AgiliFoodContainer();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }        


        //Verifica se existe usuário com email e senha apresentados
        //Se sim, define as variáveis de sessão e direciona para a devida página
        //Se não, volta para a página de login
        [HttpPost]
        public ActionResult Login(Users user)
        {
            if (ModelState.IsValid)
            {
                var users = db.Administrators.ToList();
                if (users.Count == 0)
                {

                    var item = db.Set<Users>();
                    item.Add(new Users
                    {
                        Email = "admin@admin.com",
                        Password = Hash.MD5Hash("123"),
                        Profile = 2
                    });

                    db.SaveChanges();

                    var id = db.Users.Where(x => x.Profile == 2).FirstOrDefault().IdUser;

                    var pessoa = db.Set<Administrators>();
                    pessoa.Add(new Administrators
                    {
                        Name = "RH",
                        IdUser = id
                    });

                    db.SaveChanges();

                    Session["UserID"] = id;
                    Session["Profile"] = 2;

                    return RedirectToAction("Dashboard", "Administrators");
                }
                else
                {
                    var senha = Hash.MD5Hash(user.Password);
                    var obj = db.Users.Where(a => a.Email.Equals(user.Email) && a.Password.Equals(senha)).FirstOrDefault();

                    if (obj != null)
                    {
                        Session["UserID"] = obj.IdUser.ToString();
                        Session["UserName"] = obj.Email.ToString();
                        Session["Profile"] = obj.Profile.ToString();

                        if (obj.Profile == 1)
                        {
                            return RedirectToAction("Index", "Users");
                        }
                        else if (obj.Profile == 2)
                        {
                            return RedirectToAction("Dashboard", "Administrators");
                        }
                        else if (obj.Profile == 3)
                        {
                            return RedirectToAction("Dashboard", "Restaurant");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Login");
                    }
                }
                return View(user);
            }
            return RedirectToAction("Login");
        }

        //Reinicia as variáveis de sessão e
        //redireciona o usuário para a tela de login
        public ActionResult Checkout()
        {
            if (Session["UserID"] != null)
            {
                Session["UserID"] = null;
                Session["UserName"] = null;
                Session["Profile"] = null;
            }
            return RedirectToAction("Login", "Home");
        }

        //Retorna uma imagem para a view
        public ActionResult RetrieveImage()
        {
            int id = Convert.ToInt32(Session["UserID"]);
            int profile = Convert.ToInt32(Session["Profile"]);
            byte[] cover = Image.GetProfileImageFromDataBase(id, profile, db);
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