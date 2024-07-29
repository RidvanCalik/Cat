
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CatImages.Models;
using System.Text.Json;
using System.IO;
using NuGet.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Collections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;



namespace CatImages.Controllers
{



    [Route("api/[controller]")]
    [ApiController]
    public class CatItemsController : Controller
    {
        private readonly CatContext _context;


        public CatItemsController(CatContext context)
        {
            _context = context;
        }


        static async Task<string> DownloadImageAsync(string urlParam)
        {
            string returnedImgName="";
             using (HttpClient client = new HttpClient())
            {
                try
                {
                    // API'ye GET isteği gönderme
                    HttpResponseMessage responeseCatData = await client.GetAsync(urlParam);
                    responeseCatData.EnsureSuccessStatusCode();

                    string content = await responeseCatData.Content.ReadAsStringAsync();

                    
                     using (JsonDocument doc = JsonDocument.Parse(content))
                    {

                        string catImageUrl = doc.RootElement[0].GetProperty("url").ToString();//birçok veri içeren bilgimizden sadece url key'ini alıyoruz
                        HttpResponseMessage responseCatImage = await client.GetAsync(catImageUrl);//kedi resmine istek atıyoruz
                        Console.WriteLine(responseCatImage.StatusCode);
                        byte[] catImgAsByte = await responseCatImage.Content.ReadAsByteArrayAsync();//gelen resmi byte türünde değişkene alıyoruz
                        string imgName = catImageUrl.Substring(catImageUrl.LastIndexOf('/') + 1);//Dosya ismini url'den alıyoruz
                        System.IO.File.WriteAllBytes("wwwroot\\"+imgName, catImgAsByte);//dosyayı kaydediyoruz
                        returnedImgName = imgName;
                        
                        


                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                }
            }
             return returnedImgName;
        }

        [Route("/getnewcatimage")]
        [HttpGet]
        public async Task<ActionResult<string>> GetNewCatImages()
        {
            ////Rabbit mq connections
            //var factory = new ConnectionFactory();
            //factory.HostName = "178.157.10.187";
            //factory.UserName = "app";
            //factory.Password = "U7Ob2kF5tQr1";
            //Console.WriteLine(builder.Configuration.GetConnectionString("RabbitMq"));

            //using var channel = connection.CreateModel();

            //channel.QueueDeclare(queue: "catQueue",
            //         durable: false,
            //         exclusive: false,
            //         autoDelete: false,
            //         arguments: null);





            string imgName=await DownloadImageAsync("https://api.thecatapi.com/v1/images/search");
            CatItem catItem = new Models.CatItem() { catImgName= imgName ,catScore= new Random().Next(1, 11)};
            await _context.CatItems.AddAsync(catItem);
            _context.SaveChanges();






            return imgName;
           
        }


          




        // GET: api/CatItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CatItem>>> GetCatItems()
        {

           

            return await _context.CatItems.OrderBy(item=>item.Id).Reverse().ToListAsync();
        }

        // GET: api/CatItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CatItem>> GetCatItem(int id)
        {
            var catItem = await _context.CatItems.FindAsync(id);

            if (catItem == null)
            {
                return NotFound();
            }

            return catItem;
        }

        // PUT: api/CatItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCatItem(int id, CatItem catItem)
        {
            if (id != catItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(catItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CatItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CatItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<CatItem>> PostCatItem(CatItem catItem)
        //{
        //    _context.CatItems.Add(catItem);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetCatItem", new { id = catItem.Id }, catItem);
        //}

        // DELETE: api/CatItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCatItem(int id)
        {
            var catItem = await _context.CatItems.FindAsync(id);
            if (catItem == null)
            {
                return NotFound();
            }

            _context.CatItems.Remove(catItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CatItemExists(int id)
        {
            return _context.CatItems.Any(e => e.Id == id);
        }
    }

  }
