using EntitySearch.StoreAPI.Core.Domain.Entities;
using EntitySearch.StoreAPI.Core.Infrastructures.Data.Contexts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EntitySearch.StoreAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = AppDomain.CurrentDomain.Load("EntitySearch.StoreAPI");
            services.AddMediatR(assembly);

            services.AddDbContext<StoreContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, StoreContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            ContextSeedAsync(context).Wait();

            app.UseCors();
            //app.UseHttpsRedirection();
            app.UseMvc();
        }

        private async Task ContextSeedAsync(StoreContext context)
        {
            if (false)
            {
                if (context.Products.Any())
                {
                    await SeedMoreDataAsync(context);
                }
                else
                {
                    context.Products.Add(new Product
                    {
                        Name = "Steve Madden Men's Jagwar",
                        Description = "100 % Leather / Imported / Synthetic sole / hand finished upper",
                        Value = 59.95M,
                        Ammount = 15,
                        IsPublic = true,
                        RegistrationDate = DateTime.UtcNow
                    });
                    context.Products.Add(new Product
                    {
                        Name = "Bed Sheets Queen Size Grey",
                        Description = "6 Piece 1500 Thread Count Fine Brushed Microfiber Deep Pocket Queen Sheet Set Bedding - 2 EXTRA PILLOW CASES, GREAT VALUE - Queen, Gray",
                        Value = 14.66M,
                        Ammount = 92,
                        IsPublic = true,
                        RegistrationDate = DateTime.UtcNow
                    });
                    context.Products.Add(new Product
                    {
                        Name = "1/4 cttw Diamond Stud Earrings 14K White Gold with Push-Backs and Gift Box",
                        Description = "30 Day Easy Returns.Jewelry Gift Box Included / Total Diamond Carat Weight Of The 2 Stones Is 0.25 CT / 14K White Gold / 4 Prong Basket Set Diamond Earrings / Diamond Earrings Are Handcrafted in New York",
                        Value = 89.97M,
                        Ammount = 7,
                        IsPublic = true,
                        RegistrationDate = DateTime.UtcNow
                    });
                    context.Products.Add(new Product
                    {
                        Name = "How to Day Trade for a Living",
                        Description = "A Beginner’s Guide to Trading Tools and Tactics, Money Management, Discipline and Trading Psychology",
                        Value = 13.29M,
                        Ammount = 150,
                        IsPublic = true,
                        RegistrationDate = DateTime.UtcNow
                    });
                    context.Products.Add(new Product
                    {
                        Name = "Microservice Patterns and Best Practices",
                        Description = "Explore patterns like CQRS and event sourcing to create scalable, maintainable, and testable microservices",
                        Value = 44.99M,
                        Ammount = 42,
                        IsPublic = true,
                        RegistrationDate = DateTime.UtcNow
                    });
                    context.Products.Add(new Product
                    {
                        Name = "Samsung 860 EVO 1TB 2.5 Inch SATA III Internal SSD (MZ-76E1T0B/AM) ",
                        Description = "Make sure this fits by entering your model number. / Powered by Samsung V - NAND Technology.Optimized Performance for Everyday Computing / Enhanced Performance: Sequential Read / Write speeds up to 550MB / s and 520MB / s respectively / Ideal for mainstream PCs and laptops for personal, gaming and business use / Hardware / Software Compatibility: Windows 8 / Windows 7 / Windows Server 2003(32 - bit and 64 - bit), Vista(SP1 and above), XP(SP2 and above), MAC OSX, and Linux / Included Contents: 2.5\" (7mm) SATA III (6Gb/s) SSD & User Manual (All Other Cables, Screws, Brackets Not Included). 5-Year Warranty",
                        Value = 147.99M,
                        Ammount = 23,
                        IsPublic = true,
                        RegistrationDate = DateTime.UtcNow
                    });
                    context.Products.Add(new Product
                    {
                        Name = "HP 23.8-inch FHD IPS Monitor with Tilt/Height Adjustment and Built-in Speakers (VH240a, Black)",
                        Description = "Monitor: 23.8-inch diagonal Full HD (1920x1080) micro-edge IPS; An ultra-wide viewing experience provides for seamless multi-monitor set-ups / Resolution and aspect ratio: Full HD and 16:9; 2 million pixels for crystal-clear visuals and vibrant image quality. Brightness: 250 cd/m2, Supports 100 mm standard VESA pattern mount / Response time and refresh rate: 5ms and 60Hz; Get a smooth picture that looks crisp and fluid without motion blur / Ports: HDMI, VGA, and HDCP support help you stay connected. Response time : 14 ms on/off (typical). 7 ms gray to gray with overdrive / Audio: integrated speakers; Get great audio built right in from the integrated speakers / Environmental features: Mercury-free LED backlighting, Arsenic-free monitor glass, and the low-voltage halogen design promote energy efficiency",
                        Value = 109.99M,
                        Ammount = 74,
                        IsPublic = true,
                        RegistrationDate = DateTime.UtcNow
                    });
                    context.Products.Add(new Product
                    {
                        Name = "All-new Echo Dot (3rd Gen) - Smart speaker with Alexa - Charcoal",
                        Description = "Echo Dot is our most popular voice-controlled speaker, now with improved sound and a new design. Ask Alexa to play music, answer questions, read the news, check the weather, set alarms, control compatible smart home devices, and more. Stream songs from Amazon Music, Apple Music, Spotify, Pandora, SiriusXM, and others through the improved speaker for richer and louder sound. Call and message almost anyone hands-free. Instantly drop in on other rooms in your home or make an announcement to every room with a compatible Echo device. Alexa is always getting smarter and adding new skills like tracking fitness, playing games, and more. Can hear you from across the room. And with compatible Echo devices in different rooms, you can fill your whole home with music. Pair with a second Echo Dot (3rd gen) for stereo sound, or connect to your own speakers over Bluetooth or with a 3.5 mm audio cable. Use your voice to turn on lights, adjust thermostats, lock doors, find TV shows, and more with compatible connected devices.",
                        Value = 49.99M,
                        Ammount = 88,
                        IsPublic = true,
                        RegistrationDate = DateTime.UtcNow
                    });
                    context.Products.Add(new Product
                    {
                        Name = "Add-on Blink XT Home Security Camera for Existing Blink Customer Systems",
                        Description = "ADD-ON BLINK XT CAMERA FOR EXISTING SYSTEMS (Does not include required Blink Sync Module - only works with an existing system). WEATHERPROOF FOR OUTDOOR OR INDOOR USE: Place and move your wireless Blink camera anywhere around your home both inside and out. Start off with a small system and expand to up to 10 cameras on one Blink Sync Module. BATTERY POWERED SECURITY SYSTEM: Wireless home camera with 2-year battery life, powered by 2 Lithium AA 1.5v non-rechargeable Lithium batteries (included), data is sent from IP cameras over Wi-Fi. SMART HOME CAMERA SECURITY SYSTEM WITH HD VIDEO: Simple self-install home monitoring in minutes. Easy to control wireless cameras with the free iOS & Android apps or through Alexa-enabled devices such as an Echo. FREE CLOUD STORAGE: No monthly fees or service contract required. Requires iOS 10.3 or Android 5 Lollipop or higher.",
                        Value = 119.99M,
                        Ammount = 241,
                        IsPublic = true,
                        RegistrationDate = DateTime.UtcNow
                    });
                    context.Products.Add(new Product
                    {
                        Name = "Nikon AF-S DX NIKKOR 35mm f/1.8G Lens with Auto Focus for Nikon DSLR Cameras ",
                        Description = "Make sure this fits by entering your model number. F Mount Lens/DX Format. Picture Angle with Nikon DX Format - 44 degree 52.5mm (35mm Equivalent). Rear Focusing; Manual Focus Override Aperture Range: f/1.8 to 22 ; Dimensions(approx.):Approx. 70 x 52.5 millimeter Silent Wave Motor AF System. Accepts filter type is screw on Compatible formats is dx and fx in dx crop mode Lens not zoomable  ",
                        Value = 196.95M,
                        Ammount = 21,
                        IsPublic = true,
                        RegistrationDate = DateTime.UtcNow
                    });

                    await context.SaveChangesAsync();
                }
            }
        }

        private async Task SeedMoreDataAsync(StoreContext context)
        {
            var phrases = Phrases();
            int numberOfPhrases = phrases.Count();
            for (var i = 0; i <= 120000; i++)
            {
                Random random = new Random(DateTime.UtcNow.TimeOfDay.Seconds * i);
                context.Products.Add(new Product
                {
                    Name = string.Format("{0}.", phrases[random.Next(phrases.Count())].TrimStart().TrimEnd()),
                    Description = string.Format("{0}. {1}. {2}. {3}. {4}.", phrases[random.Next(numberOfPhrases)].TrimStart().TrimEnd(), phrases[random.Next(numberOfPhrases)].TrimStart().TrimEnd(), phrases[random.Next(numberOfPhrases)].TrimStart().TrimEnd(), phrases[random.Next(numberOfPhrases)].TrimStart().TrimEnd(), phrases[random.Next(numberOfPhrases)].TrimStart().TrimEnd()),
                    Value = Convert.ToDecimal(random.Next(0, 1000).ToString() + "," + random.Next(99).ToString()),
                    Ammount = random.Next(1000),
                    IsPublic = Convert.ToBoolean(random.Next(1)),
                    RegistrationDate = DateTime.UtcNow
                });
            }

            await context.SaveChangesAsync();
        }

        private string[] Phrases()
        {
            return ("Lorem ipsum dolor sit amet, consectetur adipiscing elit.Donec tristique elit diam, vitae porta nisl molestie et. Ut non dolor dictum, pretium massa sed, vulputate erat.Cras lorem enim, ultricies at blandit in, congue et sem.Vestibulum consectetur ornare ex, id sagittis dolor suscipit in. Duis metus mi, lobortis ac dapibus at, blandit a dui. Nullam finibus faucibus nunc, at vestibulum tellus. Suspendisse vitae rutrum elit, sed rutrum metus. Suspendisse potenti. Mauris malesuada tincidunt nulla. Morbi fringilla, eros dapibus euismod condimentum, arcu nulla euismod ipsum, vitae tempor ante ex luctus est.Maecenas tincidunt mi ut est porttitor, ac posuere justo sollicitudin.Nulla non purus lacinia, finibus libero eget, vestibulum justo."
                + "Etiam vitae erat sed neque posuere efficitur eu et leo. Duis tempus, nisl nec viverra lobortis, mi diam convallis ante, ac sagittis neque nunc sed nulla.Maecenas nec tempus enim. In vestibulum tempor mi nec convallis. Suspendisse pellentesque quam quis auctor lobortis. Aenean facilisis leo purus, in pretium libero auctor eget. Fusce tincidunt, lorem ut imperdiet rhoncus, enim neque efficitur ipsum, eu iaculis velit dui quis magna.Aliquam euismod faucibus leo, vel maximus erat accumsan interdum. Vestibulum at metus sed erat rhoncus mollis.Aenean egestas vel odio luctus ultrices. Sed quis risus sit amet lacus luctus varius in sit amet nisi."
                + "Nullam nibh sem, tempor in metus sed, tincidunt semper risus. Donec auctor consectetur congue. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas.Fusce lectus mauris, molestie dictum est eu, imperdiet euismod nisi. Aliquam id blandit magna, in semper quam. Suspendisse vitae leo vitae nunc suscipit varius sed in mi.Nulla porta consectetur sapien, a luctus lectus finibus vel. Phasellus commodo sit amet orci quis posuere.Fusce dictum venenatis sapien in cursus.Mauris rhoncus mauris tincidunt tortor efficitur vehicula at tempor nunc. Donec iaculis at augue sed fringilla. Nullam at euismod tellus. Maecenas a quam congue, dignissim ante quis, commodo odio.Fusce erat arcu, aliquet ac laoreet ut, suscipit a purus. Cras egestas tempus tellus at imperdiet."
                + "Donec pellentesque vitae felis nec venenatis. Vivamus ex mi, interdum vel tristique sit amet, cursus nec dolor.Nam vel nibh sit amet nulla aliquam viverra sit amet ut purus. Donec non orci auctor, blandit augue eget, dignissim lacus.Morbi dignissim lectus massa, at egestas erat venenatis vel. Nam tincidunt diam quis posuere laoreet. Integer vestibulum vulputate varius. Donec semper enim ligula, sed bibendum ex porta porta. Donec faucibus sollicitudin libero vel ultricies. Donec eget neque et purus maximus tincidunt et non ipsum. Praesent sodales posuere pretium. Nam elementum convallis ultrices."
                + "Mauris rutrum, massa eget euismod elementum, ipsum turpis dignissim mi, ac vulputate orci erat non sapien.Nullam luctus ipsum vitae ex gravida vulputate.Donec porttitor nisi odio, ac gravida quam lobortis sit amet.Vestibulum neque justo, mattis rutrum tortor efficitur, sodales dictum odio. Vivamus mi quam, mollis in vulputate eget, pretium et eros. Vestibulum sit amet posuere massa.Praesent faucibus felis et faucibus consectetur. Sed tempus augue id mi molestie, in porta sem bibendum.Donec nisi purus, faucibus eget posuere eu, congue et felis. Curabitur non hendrerit libero, semper placerat odio. Nulla vitae vehicula enim. Mauris tempus sem libero, ac sollicitudin risus dictum at. Duis aliquet nisi velit, eget ullamcorper mauris rutrum ac. Pellentesque dictum velit felis, non elementum libero congue et. Mauris sed sem placerat, lacinia lorem et, sodales velit.").Split(".");
        }
    }
}
