using ProjetoAgili.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoAgili.Commom
{
    public class DataChange
    {
        Hash hash = new Hash();

        public static void ResetPassword(int id, AgiliFoodContainer db, ChangePasswordModel model)
        {
            Users usuario = new Users();

            if (id > 0)
            {
                var senha = Hash.MD5Hash(model.Code);
                usuario = db.Users.Where(x => x.IdUser == id && x.Password.Equals(senha)).FirstOrDefault(); ;
            }
            if (usuario != null)
            {
                var novaSenha = Hash.MD5Hash(model.Password);
                usuario.Password = novaSenha;
                
                db.Entry(usuario).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

        }

        public static void DataProfile(DataProfileModel model, int id, int profile, AgiliFoodContainer db)
        {
            List<Administrators> admin;
            List<Restaurants> restaurant;
            var proceed = false;

            if (profile == 3)
            {
                restaurant = db.Restaurants.Where(x => x.IdUser == id).ToList();
                if (restaurant.Count == 0)
                {
                    var profs = db.Set<Restaurants>();

                    profs.Add(new Restaurants { Name = model.Name, StreetAddress = model.StreetAddress, Phone = model.Phone, IdUser = id });
                    db.SaveChanges();
                }
                else
                {
                    proceed = true;
                }

                if (proceed)
                {
                    if (model.Name != null)
                        restaurant.First().Name = model.Name;
                    if (model.StreetAddress != null)
                        restaurant.First().StreetAddress = model.StreetAddress;
                    if (model.Phone != null)
                        restaurant.First().Phone = model.Phone;

                    db.Entry(restaurant.First()).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else if (profile == 2)
            {
                admin = db.Administrators.Where(x => x.IdUser == id).ToList();

                if (admin.Count == 0)
                {
                    var profs = db.Set<Administrators>();

                    profs.Add(new Administrators { Name = model.Name, IdUser = id, Phone = model.Phone });
                    db.SaveChanges();
                }
                else
                {
                    proceed = true;
                }

                if (proceed)
                {
                    if (model.Name != null)
                        admin.First().Name = model.Name;
                    if (model.Phone != null)
                        admin.First().Phone = model.Phone;

                    db.Entry(admin.First()).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }
    }
}