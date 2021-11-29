using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace BotTelegram
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            WebClient webclient = new WebClient();
            var client = HttpClientFactory.Create();

            while (!stoppingToken.IsCancellationRequested)
            {

                try
                {
                    _logger.LogInformation("Iniciando Serviço");
                    HttpResponseMessage responseApi = await client.GetAsync("https://localhost:44308/api/User");
                    using var responseStreamApi = await responseApi.Content.ReadAsStreamAsync();

                    if (!responseApi.IsSuccessStatusCode)
                    {
                        try
                        {
                            var bot = new Telegram.Bot.TelegramBotClient("2147153362:AAE50iK5OOYYa4GP876JtdQIih4WZhAcf1o");
                            await bot.SendTextMessageAsync("967900340", "Api não está respondendo");
                            _logger.LogInformation("TELEGRAM SEND");


                        }
                        catch (Exception e)
                        {
                            _logger.LogError("ERROR :", e);

                        }

                    }

                }
                catch
                {
                    try
                    {
                        var bot = new Telegram.Bot.TelegramBotClient("2147153362:AAE50iK5OOYYa4GP876JtdQIih4WZhAcf1o");
                        await bot.SendTextMessageAsync("967900340", "Api Desligada");
                        _logger.LogInformation("TELEGRAM SEND");


                    }
                    catch (Exception e)
                    {
                        _logger.LogError("ERROR :", e);

                    }
                }


                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
