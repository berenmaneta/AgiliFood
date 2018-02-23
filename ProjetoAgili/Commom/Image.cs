using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ProjetoAgili.Commom
{
    public class Image
    {
        public static void UploadImage(HttpPostedFileBase file, int profile, int id, AgiliFoodContainer db)
        {
            byte[] foto;
            if (file != null)
            {
                foto = Image.ConvertToBytes(file);

                if (profile == 2)
                {
                    var admin = db.Administrators.Where(x => x.IdUser == id).FirstOrDefault();
                    admin.Photo = foto;
                    db.Entry(admin).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    var rest = db.Restaurants.Where(x => x.IdUser == id).FirstOrDefault();
                    rest.Photo = foto;
                    db.Entry(rest).State = System.Data.Entity.EntityState.Modified;                  
                }
                db.SaveChanges();
            }

        }

        public static byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }

        public static byte[] GetImageDish(int id, AgiliFoodContainer db)
        {
            byte[] cover;
            cover = db.Dishes.Where(a => a.IdDish == id).ToList().FirstOrDefault().Photo;
            return cover;
        }

        public static byte[] GetImageRestaurant(int id, AgiliFoodContainer db)
        {
            byte[] cover;
            cover = db.Restaurants.Where(a => a.IdRestaurant == id).ToList().FirstOrDefault().Photo;
            return cover;
        }

        public static byte[] GetProfileImageFromDataBase(int id, int profile, AgiliFoodContainer db)
        {
            byte[] cover;
            if(profile == 2)
                cover = db.Administrators.Where(a => a.IdUser == id).ToList().FirstOrDefault().Photo;
            else
                cover = db.Restaurants.Where(a => a.IdUser == id).ToList().FirstOrDefault().Photo;
            return cover;
        }
    }
}