using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data.SqlClient;

namespace HCImageDownload
{
    class Program
    {
        static void Main(string[] args)
        {

            DownloadImages();
        }


        static void DownloadImages()
        {
            

            string connectionString = "Data Source=RHFSQLDEV;Initial Catalog=HCWeb;Integrated Security=SSPI;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(
                             "SELECT [SKU] ,[ImageURL] FROM [HCWeb].[dbo].[Items_Init] WHERE ImageURL <> '' ", connection))
                {
                    connection.Open();
                    SqlDataReader result = (SqlDataReader)command.ExecuteReader();

                    while (result.Read())
                    {
                        SaveImage(result["SKU"] + "_small.jpg", ImageFormat.Jpeg, result["ImageURL"] + "?wid=300&hei=300");
                        SaveImage(result["SKU"] + "_medium.jpg", ImageFormat.Jpeg, result["ImageURL"] + "?wid=750&hei=750");
                        SaveImage(result["SKU"] + ".jpg", ImageFormat.Jpeg, result["ImageURL"] + "?wid=1500&hei=1500");


                    }
                }
            }
        }

        static void SaveImage(string filename, ImageFormat format, string imageUrl)
        {

            WebClient client = new WebClient();
            Stream stream = client.OpenRead(imageUrl);
            Bitmap bitmap; bitmap = new Bitmap(stream);

            if (bitmap != null)
                bitmap.Save("Images\\" + filename, format);

            stream.Flush();
            stream.Close();
            client.Dispose();
        }
    }
}
