using DataEntities;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

using static System.Console;
using System.Text;
using System.Text.Json.Serialization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DataCommon
{
    public static class HttpExtensions
    {
        public static object CopyData(this object named, object data)
        {
            named.GetType().GetProperties().ToList().ForEach(p => data.GetType().GetProperties().Where(namedProp => namedProp.Name == p.Name).First().SetValue(named, p.GetValue(data)));
            return named;
        }
    }

    public class CustomerGenerator<TItem>
    where TItem : SaleItem
    {


        private static string MANS_NAMES_INPUT = "А АаронАбрамАвазАвгустинАвраамАгапАгапитАгатАгафонАдамАдрианАзаматАзатАзизАидАйдарАйратАкакийАкимАланАлександрАлексейАлиАликАлимАлиханАлишерАлмазАльбертАмирАмирамАмиранАнарАнастасийАнатолийАнварАнгелАндрейАнзорАнтонАнфимАрамАристархАркадийАрманАрменАрсенАрсенийАрсланАртёмАртемийАртурАрхипАскарАсланАсханАсхатАхметАшот Б БахрамБенджаминБлезБогданБорисБориславБрониславБулат В ВадимВалентинВалерийВальдемарВарданВасилийВениаминВикторВильгельмВитВиталийВладВладимирВладиславВладленВласВсеволодВячеслав Г ГавриилГамлетГарриГеннадийГенриГенрихГеоргийГерасимГерманГерманнГлебГордейГригорийГустав Д ДавидДавлатДамирДанаДаниилДанилДанисДаниславДаниэльДаниярДарийДауренДемидДемьянДенисДжамалДжанДжеймсДжереми ИеремияДжозефДжонатанДикДинДинарДиноДмитрийДобрыняДоминик Е ЕвгенийЕвдокимЕвсейЕвстахийЕгорЕлисейЕмельянЕремейЕфимЕфрем Ж ЖданЖерарЖигер З ЗакирЗаурЗахарЗенонЗигмундЗиновийЗурабЗуфар И ИбрагимИванИгнатИгнатийИгорьИероним ДжеромИисусИльгизИльнурИльшатИльяИльясИмранИннокентийИраклийИсаакИсаакийИсидорИскандерИсламИсмаилИтан К КазбекКамильКаренКаримКарлКимКирКириллКлаусКлимКонрадКонстантинКореКорнелийКристианКузьма Л ЛаврентийЛадоЛевЛенарЛеонЛеонардЛеонидЛеопольдЛоренсЛукаЛукиллианЛукьянЛюбомирЛюдвигЛюдовикЛюций М МаджидМайклМакарМакарийМаксимМаксимилианМаксудМансурМарМаратМаркМарсельМартин МартынМатвейМахмудМикаМикулаМилославМиронМирославМихаилМоисейМстиславМуратМуслимМухаммедМэтью Н НазарНаильНариманНатанНесторНикНикитаНикодимНиколаНиколайНильсНурлан О ОгюстОлегОливерОрестОрландоОсип ИосифОскарОсманОстапОстин П ПавелПанкратПатрикПедроПерриПётрПлатонПотапПрохор Р РавильРадийРадикРадомирРадославРазильРаильРаифРайанРаймондРамазанРамзесРамизРамильРамонРанельРасимРасулРатиборРатмирРаушанРафаэльРафикРашидРинат РенатРичардРобертРодимРодионРожденРоланРоманРостиславРубенРудольфРусланРустамРэй С СавваСавелийСаидСалаватСаматСамвелСамирСамуилСанжарСаниСаянСвятославСевастьянСемёнСерафимСергейСидорСимбаСоломонСпартакСтаниславСтепанСулейманСултанСурен Т ТагирТаирТайлерТалгатТамазТамерланТарасТахирТигранТимофейТимурТихонТомасТрофим У УинслоуУмарУстин Ф ФазильФаридФархадФёдорФедотФеликсФилиппФлорФомаФредФридрих Х ХабибХакимХаритонХасан Ц ЦезарьЦефасЦецилий СесилЦицерон Ч ЧарльзЧеславЧингиз Ш ШамильШарльШерлок Э ЭдгарЭдуардЭльдарЭмильЭминЭрикЭркюльЭрминЭрнестЭузебио Ю ЮлианЮлийЮнусЮрийЮстинианЮстус Я ЯковЯнЯромирЯрослав";
        private static string WOMANS_NAMES_INPUT = "А АваАвгустаАвгустинаАвдотьяАврораАгапияАгатаАгафьяАглаяАгнияАгундаАдаАделаидаАделинаАдельАдиляАдрианаАзаАзалияАзизаАидаАишаАйАйаруАйгеримАйгульАйлинАйнагульАйнурАйсельАйсунАйсылуАксиньяАланаАлевтинаАлександраАлексияАлёнаАлестаАлинаАлисаАлияАллаАлсуАлтынАльбаАльбинаАльфияАляАмалияАмальАминаАмираАнаитАнастасияАнгелинаАнжелаАнжеликаАнисьяАнитаАннаАнтонинаАнфисаАполлинарияАрабеллаАриаднаАрианаАриандаАринаАрияАсельАсияАстридАсяАфинаАэлитаАяАяна Б БаженаБеатрисБелаБелиндаБелла БэллаБертаБогданаБоженаБьянка В ВалентинаВалерияВандаВанессаВарвараВасилинаВасилисаВенераВераВероникаВестаВетаВикторинаВикторияВиленаВиолаВиолеттаВитаВиталина ВиталияВладаВладанаВладислава Г ГабриэллаГалинаГалияГаянаГаянэГенриеттаГлафираГоарГретаГульзираГульмираГульназГульнараГульшатГюзель Д ДалидаДамираДанаДаниэлаДанияДараДаринаДарьяДаянаДжамиляДженнаДженниферДжессикаДжиневраДианаДильназДильнараДиляДилярамДинаДинараДолоресДоминикаДомнаДомника Е ЕваЕвангелинаЕвгенияЕвдокияЕкатеринаЕленаЕлизаветаЕсенияЕя Ж ЖаклинЖаннаЖансаяЖасминЖозефинаЖоржина З ЗабаваЗаираЗалинаЗамираЗараЗаремаЗаринаЗемфираЗинаидаЗитаЗлатаЗлатославаЗорянаЗояЗульфияЗухра И Иветта ИветаИзабеллаИлинаИллирикаИлонаИльзираИлюзаИнгаИндираИнессаИннаИоаннаИраИрадаИраидаИринаИрмаИскраИя К КамилаКамиллаКараКареКаримаКаринаКаролинаКираКлавдияКлараКонстанцияКораКорнелияКристинаКсения Л ЛадаЛанаЛараЛарисаЛаураЛейлаЛеонаЛераЛесяЛетаЛианаЛидияЛизаЛикаЛилиЛилианаЛилитЛилияЛинаЛиндаЛиораЛираЛияЛолаЛолитаЛораЛуизаЛукерьяЛукияЛунаЛюбаваЛюбовьЛюдмилаЛюсильЛюсьенаЛюцияЛючеЛяйсанЛяля М МавилеМавлюдаМагдаМагдалeнаМадинаМадленМайяМакарияМаликаМараМаргаритаМарианнаМарикаМаринаМарияМариямМартаМарфаМеланияМелиссаМехриМикаМилаМиладаМиланаМиленМиленаМилицаМилославаМинаМираМирославаМирраМихримахМишельМияМоникаМуза Н НадеждаНаиляНаимаНанаНаомиНаргизаНатальяНеллиНеяНикаНикольНинаНинельНоминаНоннаНораНурия О ОдеттаОксанаОктябринаОлесяОливияОльгаОфелия П ПавлинаПамелаПатрицияПаулаПейтонПелагеяПеризатПлатонидаПолинаПрасковья Р РавшанаРадаРазинаРаиляРаисаРаифаРалинаРаминаРаянаРебеккаРегинаРезедаРенаРенатаРианаРианнаРикардаРиммаРинаРитаРогнедаРозаРоксанаРоксоланаРузалияРузаннаРусалинаРусланаРуфинаРуфь С СабинаСабринаСажидаСаидаСалимаСаломеяСальмаСамираСандраСанияСараСатиСаулеСафияСафураСаянаСветланаСевараСеленаСельмаСерафимаСильвияСимонаСнежанаСоняСофьяСтеллаСтефанияСусанна Т ТаисияТамараТамилаТараТатьянаТаяТаянаТеонаТерезаТеяТинаТиффаниТомирисТораТэмми У УльянаУмаУрсулаУстинья Ф ФазиляФаинаФаридаФаризаФатимаФедораФёклаФелиситиФелицияФерузаФизалияФирузаФлораФлорентинаФлоренция ФлоренсФлорианаФредерикаФрида Х ХадияХилариХлояХюррем Ц ЦаганаЦветанаЦецилия СесилияЦиара Сиара Ч ЧелсиЧеславаЧулпан Ш ШакираШарлоттаШахинаШейлаШеллиШерил Э ЭвелинаЭвитаЭлеонораЭлианаЭлизаЭлинаЭллаЭльвинаЭльвираЭльмираЭльнараЭляЭмилиЭмилияЭммаЭнжеЭрикаЭрминаЭсмеральдаЭсмираЭстерЭтельЭтери Ю ЮлианнаЮлияЮнаЮнияЮнона Я ЯдвигаЯнаЯнинаЯринаЯрославаЯсмина";
        private static string MANS_SECONDNAMES_INPUT = "АлексеевичАнатольевичАндреевичАнтоновичАркадьевичАртемовичБедросовичБогдановичБорисовичВалентиновичВалерьевичВасильевичВикторовичВитальевичВладимировичВладиславовичВольфовичВячеславовичГеннадиевичГеоргиевичГригорьевичДаниловичДенисовичДмитриевичЕвгеньевичЕгоровичЕфимовичИвановичИванычИгнатьевичИгоревичИльичИосифовичИсааковичКирилловичКонстантиновичЛеонидовичЛьвовичМаксимовичМатвеевичМихайловичНиколаевичОлеговичПавловичПалычПетровичПлатоновичРобертовичРомановичСанычСевериновичСеменовичСергеевичСтаниславовичСтепановичТарасовичТимофеевичФедоровичФеликсовичФилипповичЭдуардовичЮрьевичЯковлевичЯрославович";
        private static string MANS_SURNAMES_INPUT = "СмирновИвановКузнецовСоколовПоповЛебедевКозловНовиковМорозовПетровВолковСоловьёвВасильевЗайцевПавловСемёновГолубевВиноградовБогдановВоробьёвФёдоровМихайловБеляевТарасовБеловКомаровОрловКиселёвМакаровАндреевКовалёвИльинГусевТитовКузьминКудрявцевБарановКуликовАлексеевСтепановЯковлевСорокинСергеевРомановЗахаровБорисовКоролёвГерасимовПономарёвГригорьевЛазаревМедведевЕршовНикитинСоболевРябовПоляковЦветковДаниловЖуковФроловЖуравлёвНиколаевКрыловМаксимовСидоровОсиповБелоусовФедотовДорофеевЕгоровМатвеевБобровДмитриевКалининАнисимовПетуховАнтоновТимофеевНикифоровВеселовФилипповМарковБольшаковСухановМироновШиряевАлександровКоноваловШестаковКазаковЕфимовДенисовГромовФоминДавыдовМельниковЩербаковБлиновКолесниковКарповАфанасьевВласовМасловИсаковТихоновАксёновГавриловРодионовКотовГорбуновКудряшовБыковЗуевТретьяковСавельевПановРыбаковСуворовАбрамовВороновМухинАрхиповТрофимовМартыновЕмельяновГоршковЧерновОвчинниковСелезнёвПанфиловКопыловМихеевГалкинНазаровЛобановЛукинБеляковПотаповНекрасовХохловЖдановНаумовШиловВоронцовЕрмаковДроздовИгнатьевСавинЛогиновСафоновКапустинКирилловМоисеевЕлисеевКошелевКостинГорбачёвОреховЕфремовИсаевЕвдокимовКалашниковКабановНосковЮдинКулагинЛапинПрохоровНестеровХаритоновАгафоновМуравьёвЛарионовФедосеевЗиминПахомовШубинИгнатовФилатовКрюковРоговКулаковТерентьевМолчановВладимировАртемьевГурьевЗиновьевГришинКононовДементьевСитниковСимоновМишинФадеевКомиссаровМамонтовНосовГуляевШаровУстиновВишняковЕвсеевЛаврентьевБрагинКонстантиновКорниловАвдеевЗыковБирюковШараповНиконовЩукинДьячковОдинцовСазоновЯкушевКрасильниковГордеевСамойловКнязевБеспаловУваровШашковБобылёвДоронинБелозёровРожковСамсоновМясниковЛихачёвБуровСысоевФомичёвРусаковСтрелковГущинТетеринКолобовСубботинФокинБлохинСеливерстовПестовКондратьевСилинМеркушевЛыткинТуров";


        public static List<string> MANS_NAMES = GetManNames();
        public static List<string> MANS_SURNAMES = GetManSurnames();
        public static List<string> MANS_LASTNAMES = GetManLastnames();

        public static ProductCustomer<TItem> CreateCustomer()
        {
            int i1 = GetRandom(MANS_NAMES.Count() - 1);
            int i2 = GetRandom(MANS_SURNAMES.Count() - 1);
            int i3 = GetRandom(MANS_LASTNAMES.Count() - 1);
            if (i1 < 0 || i2 < 0 || i3 < 0)
            {
                throw new Exception("Индекс не может быть меньше нуля");
            }
            //Writing.ToConsole($"{i1},{i2},{i3}");
            string[] names = MANS_NAMES.ToArray();
            string name = names[i1];
            string[] surnames = MANS_SURNAMES.ToArray();
            string surname = surnames[i2];
            string[] lastnames = MANS_LASTNAMES.ToArray();
            string lastname = lastnames[i3];
            return new ProductCustomer<TItem>()
            {
                FirstName = name,
                PhoneNumber = $"7-{GetRandom(9)}{GetRandom(9)}{GetRandom(9)}-{GetRandom(9)}{GetRandom(9)}{GetRandom(9)}-{GetRandom(9)}{GetRandom(9)}{GetRandom(9)}{GetRandom(9)}",
                LastName = lastname

            };
        }

        static int GetRandom(int max)
        {
            int res = new Random().Next(max);
            return res == 0 ? 1 : res;
        }

        public static List<string> GetManNames()
        {
            List<string> names = new List<string>();
            foreach (string text in MANS_NAMES_INPUT.Split(" "))
            {
                if (text.Length > 1)
                {
                    names.AddRange(new List<string>(Naming.SplitName(text)));
                }
            }
            return names;
        }
        public static List<string> GetManLastnames()
        {
            List<string> names = new List<string>();
            foreach (string text in MANS_SECONDNAMES_INPUT.Split(" "))
            {
                if (text.Length > 1)
                {
                    names.AddRange(new List<string>(Naming.SplitName(text)));
                }
            }
            return names;
        }
        public static List<string> GetManSurnames()
        {
            List<string> names = new List<string>();
            foreach (string text in MANS_SURNAMES_INPUT.Split(" "))
            {
                if (text.Length > 1)
                {
                    names.AddRange(new List<string>(Naming.SplitName(text)));
                }
            }
            return names;
        }
        public static List<string> GetWomanNames()
        {
            List<string> names = new List<string>();
            foreach (string text in WOMANS_NAMES_INPUT.Split(" "))
            {
                if (text.Length > 1)
                {
                    names.AddRange(new List<string>(Naming.SplitName(text)));
                }
            }
            return names;
        }

        /// <summary>
        /// Перечисление стилей записи идентификаторов
        /// </summary>
        public enum NamingStyles
        {
            Capital, Kebab, Snake, Camel
        }

        /// <summary>
        /// Реализует методы работы с идентификаторами и стилями записи
        /// </summary>
        public class Naming
        {
            private static string SPEC_CHARS = ",.?~!@#$%^&*()-=+/\\[]{}'\";:\t\r\n";
            private static string RUS_CHARS = "ЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ" + "ёйцукенгшщзхъфывапролджэячсмитьбю";
            private static string DIGIT_CHARS = "0123456789";
            private static string ENG_CHARS = "qwertyuiopasdfghjklzxcvbnm" + "QWERTYUIOPASDFGHJKLZXCVBNM";




            /// <summary>
            /// Метод разбора идентификатора на модификаторы 
            /// </summary>
            /// <param name="name"> идентификатор </param>
            /// <returns> модификаторы </returns>
            public static string[] SplitName(string name)
            {
                NamingStyles style = ParseStyle(name);
                switch (style)
                {
                    case NamingStyles.Kebab: return SplitKebabName(name);
                    case NamingStyles.Snake: return SplitSnakeName(name);
                    case NamingStyles.Capital: return SplitCapitalName(name);

                    case NamingStyles.Camel: return SplitCamelName(name);
                    default:
                        throw new Exception($"Не удалось разобрать идентификатор {name}.");
                }
            }


            /// <summary>
            /// Запись идентификатора в CapitalStyle
            /// </summary>
            /// <param name="lastname"> идентификатор </param>
            /// <returns>идентификатор в CapitalStyle</returns>
            public static string ToCapitalStyle(string lastname)
            {
                if (string.IsNullOrEmpty(lastname)) return lastname;
                string[] ids = SplitName(lastname);
                return ToCapitalStyle(ids);
            }
            public static string ToCapitalStyle(string[] ids)
            {
                string name = "";
                foreach (string id in ids)
                {
                    name += id.Substring(0, 1).ToUpper() + id.Substring(1).ToLower();
                }
                return name;
            }


            /// <summary>
            /// Запись идентификатора в CamelStyle
            /// </summary>
            /// <param name="lastname"> идентификатор </param>
            /// <returns>идентификатор в CamelStyle</returns>
            public static string ToCamelStyle(string lastname)
            {
                string name = ToCapitalStyle(lastname);
                return name.Substring(0, 1).ToLower() + name.Substring(1);
            }




            /// <summary>
            /// Запись идентификатора в KebabStyle
            /// </summary>
            /// <param name="lastname"> идентификатор </param>
            /// <returns>идентификатор в KebabStyle</returns>
            public static string ToKebabStyle(string lastname)
            {
                string name = "";
                foreach (string id in SplitName(lastname))
                {
                    name += "-" + id.ToLower();
                }
                return name.Substring(1);
            }





            /// <summary>
            /// Запись идентификатора в SnakeStyle
            /// </summary>
            /// <param name="lastname"> идентификатор </param>
            /// <returns>идентификатор в SnakeStyle</returns>
            public static string ToSnakeStyle(string lastname)
            {
                string name = "";
                string[] names = SplitName(lastname);
                foreach (string id in names)
                {
                    name += "_" + id.ToLower();
                }
                return name.Substring(1);
            }


            /// <summary>
            /// Метод разбора идентификатора записанного в CapitalStyle на модификаторы 
            /// </summary>
            /// <param name="name"> идентификатор записанный в CapitalStyle </param>
            /// <returns> модификаторы </returns>
            public static string[] SplitCapitalName(string name)
            {
                List<string> ids = new List<string>();
                string word = "";
                bool WasUpper = false;
                foreach (char ch in name)
                {
                    if (IsUpper(ch) && WasUpper == false)
                    {
                        if (word != "")
                        {
                            ids.Add(word);
                        }
                        word = "";
                        WasUpper = true;
                    }
                    WasUpper = false;
                    word += (ch + "");
                }
                if (word != "")
                {
                    ids.Add(word);
                }
                word = "";
                return ids.ToArray();
            }


            /// <summary>
            /// Метод разбора идентификатора записанного в DollarStyle на модификаторы 
            /// </summary>
            /// <param name="name"> идентификатор записанный в DollarStyle </param>
            /// <returns> модификаторы </returns>
            public static string[] SplitDollarName(string name)
            {
                List<string> ids = new List<string>();
                string word = "";
                bool first = true;
                foreach (char ch in name)
                {
                    if (first)
                    {
                        first = false;
                        continue;
                    }
                    if (IsUpper(ch))
                    {
                        if (word != "")
                        {
                            ids.Add(word);
                        }
                        word = "";
                    }
                    word += (ch + "");
                }
                if (word != "")
                {
                    ids.Add(word);
                }
                word = "";
                return ids.ToArray();
            }


            /// <summary>
            /// Метод разбора идентификатора записанного в CamelStyle на модификаторы 
            /// </summary>
            /// <param name="name"> идентификатор записанный в CamelStyle </param>
            /// <returns> модификаторы </returns>
            public static string[] SplitCamelName(string name)
            {
                List<string> ids = new List<string>();
                string word = "";
                foreach (char ch in name)
                {
                    if (IsUpper(ch))
                    {
                        if (word != "")
                        {
                            ids.Add(word);
                        }
                        word = "";
                    }
                    word += (ch + "");
                }
                if (word != "")
                {
                    ids.Add(word);
                }
                word = "";
                return ids.ToArray();
            }


            /// <summary>
            /// Метод разбора идентификатора записанного в SnakeStyle на модификаторы 
            /// </summary>
            /// <param name="name"> идентификатор записанный в SnakeStyle </param>
            /// <returns> модификаторы </returns>
            public static string[] SplitSnakeName(string name)
            {
                return name.Split("_");
            }


            /// <summary>
            /// Метод разбора идентификатора записанного в KebabStyle на модификаторы 
            /// </summary>
            /// <param name="name"> идентификатор записанный в KebabStyle </param>
            /// <returns> модификаторы </returns>
            public static string[] SplitKebabName(string name)
            {
                return name.Split("-");
            }


            /// <summary>
            /// Метод определния стиля записи идентификатора
            /// </summary>
            /// <param name="name"> идентификатор </param>
            /// <returns> стиль записи </returns>
            public static NamingStyles ParseStyle(string name)
            {
                if (IsCapitalStyle(name))
                    return NamingStyles.Capital;
                if (IsKebabStyle(name))
                    return NamingStyles.Kebab;
                if (IsSnakeStyle(name))
                    return NamingStyles.Snake;

                if (IsCamelStyle(name))
                    return NamingStyles.Camel;

                throw new Exception($"Стиль идентификатора {name} не определён.");
            }


            /// <summary>
            /// Проверка сивола на принадлежность с множеству цифровых символов
            /// </summary>
            /// <param name="ch"> символ </param>
            /// <returns>true, если символ цифровой</returns>
            public static bool IsDigit(char ch)
            {
                return Contains(DIGIT_CHARS, ch);
            }


            /// <summary>
            /// Проверка сивола на принадлежность с множеству символов русского алфавита
            /// </summary>
            /// <param name="ch"> символ </param>
            /// <returns>true, если символ из русского алфавита </returns>
            public static bool IsCharacter(char ch)
            {
                return IsRussian(ch) || IsEnglish(ch);
            }


            /// <summary>
            /// Проверка сивола на принадлежность с множеству символов русского алфавита
            /// </summary>
            /// <param name="ch"> символ </param>
            /// <returns>true, если символ из русского алфавита </returns>
            public static bool IsRussian(char ch)
            {
                return Contains(RUS_CHARS, ch);
            }


            /// <summary>
            /// Проверка сивола на принадлежность с множеству символов русского алфавита
            /// </summary>
            /// <param name="ch"> символ </param>
            /// <returns>true, если символ из русского алфавита </returns>
            public static bool IsEnglish(char ch)
            {
                return Contains(ENG_CHARS, ch);
            }


            /// <summary>
            /// Проверка принадлежности символа к строке
            /// </summary>
            /// <param name="text"></param>
            /// <param name="ch"></param>
            /// <returns></returns>
            public static bool Contains(string text, char ch)
            {
                bool result = false;
                foreach (char rch in text)
                {
                    if (rch == ch)
                    {
                        result = true;
                        break;
                    }
                }
                return result;
            }


            /// <summary>
            /// Метод проверки символа на принадлежность к верхнему регистру
            /// </summary>
            /// <param name="ch"> символ </param>
            /// <returns> true, если принадлежит верхнему регистру </returns>
            public static bool IsUpper(char ch)
            {
                return (ch + "") == (ch + "").ToUpper();
            }


            /// <summary>
            /// Проверка стиля записи CapitalStyle( UserId )
            /// </summary>
            /// <param name="name"> идентификатор </param>
            /// <returns> true, если идентификатор записан в CapitalStyle </returns>
            public static bool IsCapitalStyle(string name)
            {
                bool startedWithUpper = (name[0] + "") == (name[0] + "").ToUpper();
                bool containsSpecCharaters = name.IndexOf("_") != -1 || name.IndexOf("$") != -1;
                return startedWithUpper && !containsSpecCharaters;
            }


            /// <summary>
            /// Проверка стиля записи SnakeStyle( user_id, USER_ID )
            /// </summary>
            /// <param name="name"> идентификатор </param>
            /// <returns> true, если идентификатор записан в SnakeStyle </returns>
            public static bool IsSnakeStyle(string name)
            {
                bool upperCase = IsUpper(name[0]);
                bool startsWithCharacter = IsCharacter(name[0]);
                char separatorCharacter = '_';
                string anotherChars = new String(SPEC_CHARS).Replace(separatorCharacter + "", "");
                bool containsAnotherSpecChars = false;
                bool containsAnotherCase = false;
                bool containsDoubleSeparator = false;
                bool lastCharWasSeparator = false;
                if (startsWithCharacter == false)
                {
                    return !containsDoubleSeparator && !containsAnotherCase && startsWithCharacter && !containsAnotherSpecChars && !containsAnotherCase;
                }
                else
                {
                    for (int i = 1; i < name.Length; i++)
                    {
                        if (Contains(anotherChars, name[i]))
                        {
                            containsAnotherSpecChars = true;
                            break;
                        }
                        if (name[i] != separatorCharacter)
                        {
                            if (IsUpper(name[i]) != upperCase)
                            {
                                containsAnotherCase = true;
                                break;
                            }
                            lastCharWasSeparator = false;
                        }
                        else
                        {
                            if (lastCharWasSeparator)
                            {
                                containsDoubleSeparator = true;
                                break;
                            }
                            lastCharWasSeparator = true;
                        }
                    }
                }
                return !containsDoubleSeparator && !containsAnotherCase && startsWithCharacter && !containsAnotherSpecChars && !containsAnotherCase;
            }


            /// <summary>
            /// Проверка стиля записи CamelStyle( userId  )
            /// </summary>
            /// <param name="name"> идентификатор </param>
            /// <returns> true, если идентификатор записан в CamelStyle </returns>
            public static bool IsCamelStyle(string name)
            {
                return IsCapitalStyle(name.Substring(0, 1).ToUpper() + name.Substring(1)) && !IsUpper(name[0]) && IsCharacter(name[0]);
            }


            /// <summary>
            /// Проверка стиля записи DollarStyle( $userId  )
            /// </summary>
            /// <param name="name"> идентификатор </param>
            /// <returns> true, если идентификатор записан в DollarStyle </returns>
            public static bool IsDollarStyle(string name)
            {
                return IsCamelStyle(name.Substring(1)) && name[0] == '$';
            }


            /// <summary>
            /// Проверка стиля записи KebabStyle( user-id, USER-ID )
            /// </summary>
            /// <param name="name"> идентификатор </param>
            /// <returns> true, если идентификатор записан в KebabStyle </returns>
            public static bool IsKebabStyle(string name)
            {
                bool upperCase = IsUpper(name[0]);
                bool startsWithCharacter = IsCharacter(name[0]);
                char separatorCharacter = '-';
                string anotherChars = new String(SPEC_CHARS).Replace(separatorCharacter + "", "");
                bool containsAnotherSpecChars = false;
                bool containsAnotherCase = false;
                bool containsDoubleSeparator = false;
                bool lastCharWasSeparator = false;
                if (startsWithCharacter == false)
                {
                    return !containsDoubleSeparator && !containsAnotherCase && startsWithCharacter && !containsAnotherSpecChars && !containsAnotherCase;
                }
                else
                {
                    for (int i = 1; i < name.Length; i++)
                    {
                        if (Contains(anotherChars, name[i]))
                        {
                            containsAnotherSpecChars = true;
                            break;
                        }
                        if (name[i] != separatorCharacter)
                        {
                            if (IsUpper(name[i]) != upperCase)
                            {
                                containsAnotherCase = true;
                                break;
                            }
                            lastCharWasSeparator = false;
                        }
                        else
                        {
                            if (lastCharWasSeparator)
                            {
                                containsDoubleSeparator = true;
                                break;
                            }
                            lastCharWasSeparator = true;
                        }
                    }
                }
                return !containsDoubleSeparator && !containsAnotherCase && startsWithCharacter && !containsAnotherSpecChars && !containsAnotherCase;
            }
        }
    }
    public interface IDeliveryDbContextInitiallizer<TCustomer, TItem, THolder>
        where TCustomer : ProductCustomer<TItem>
        where TItem : SaleItem
        where THolder : ProductHolder<TItem>
    {

        /// <summary>
        /// Возвращает статистику типа ключ-значение
        /// ключ-наименование сущности
        /// ключ-кол-во выполненных операций
        /// </summary>
        /// <param name="context">Контекст данных с проверенной структурой</param>
        /// <param name="contentPath">Путь к директории ресурсов</param>
        /// <returns></returns>
        public IDictionary<string, int> Init(DeliveryDbContext<TCustomer, TItem, THolder> context, string contentPath);
        public int InitCustomers(DeliveryDbContext<TCustomer, TItem, THolder> context, int customersCount);
        public int InitHolders(DeliveryDbContext<TCustomer, TItem, THolder> context);
        public int InitProducts(DeliveryDbContext<TCustomer, TItem, THolder> context, string contentPath);
    }
    
    [Controller]
    public class DeliveryDbContextInitiallizer : 
        DeliveryDbContextInitiallizer<PhoneCustomer, PhoneItem, PhoneHolder>,
        IDisposable
    {
        public DeliveryDbContextInitiallizer()
        {
        }

        public void Dispose() => WriteLine("Disposed");
    }

    public class DeliveryDbContextInitiallizer<TCustomer, TItem, THolder>
        : IDeliveryDbContextInitiallizer<TCustomer, TItem, THolder>
        where TCustomer : ProductCustomer<TItem>
        where TItem : SaleItem
        where THolder : ProductHolder<TItem>

    {
        ILogger<DeliveryDbContextInitiallizer<TCustomer, TItem, THolder>> Logger = Factory.GetLogger<DeliveryDbContextInitiallizer<TCustomer, TItem, THolder>>();
        public virtual IDictionary<string, int> Init(DeliveryDbContext<TCustomer, TItem, THolder> context, string contentPath)
        {             
            Dictionary<string, int> result = new Dictionary<string, int>();
            try
            {
                result["Products"] = this.InitProducts(context, contentPath);
                result["Holders"] = this.InitHolders(context);
                result["Transport"] = this.InitTransport(context);
                result["Customers"] = this.InitCustomers(context, 10);
            }
            catch (Exception ex)
            {
                Logger.LogError(""+ex);
            }
            finally
            {
                Logger.LogInformation($"Создано объектов() => {SerializeObject(result)}");
            }



            return result;
        }

        private string SerializeObject(object result )
        {
            return System.Text.Json.JsonSerializer.Serialize(result);
        }

        private int InitTransport(DeliveryDbContext<TCustomer, TItem, THolder> context)
        {
            if (context.Transports.Count() > 0)
            {
                return 0;
            }
            else
            {
                for (var i = 0; i <= 100; i++)
                {
                    context.Transports.Add(new ProductTransport()
                    {
                        Latitude = 100,
                        Longitude = 100
                    });

                }

            }
            return context.SaveChanges();
        }

        public int InitCustomers(DeliveryDbContext<TCustomer, TItem, THolder> context, int customersCount)
        {
            //ProductCustomer.CustomerGenerator.CreateCustomer()
            int n = customersCount - context.Customers.Count();
            if (n > 0)
            {
                for (int i = 0; i < 100; i++)
                {
                    var cust = CustomerGenerator<TItem>.CreateCustomer();
                    var resolved = Resolve(cust);
                    context.Customers.Add(resolved);
                    context.SaveChanges();
                }
            }
            return n;
        }

        private TCustomer Resolve(ProductCustomer<TItem> ProductCustomer)
        {
            return (TCustomer)typeof(TCustomer).GetConstructors().FirstOrDefault().Invoke(new object[0]).CopyData(ProductCustomer);
        }


        /// <summary>
        /// Получение случайной последовательности символов
        /// </summary>
        /// <returns> последовательность символов </returns>
        private string RandomString(int _keylength)
        {
            Random random = new Random();
            string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, _keylength)
                                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public int InitHolders(DeliveryDbContext<TCustomer, TItem, THolder> _deliveryDbContext)
        {
            if (_deliveryDbContext.Holders.Count() > 0)
                return 0;
            List<string> locations = new List<string>()
            {
                "186632, г.Санкт-Петербург, Комендантский проспект, № 22, корпус1",
                "188512, Санкт-Петербург, город Ломоносов, Лесная , № 23, литер А",
                "189610, Санкт-Петербург, город Кронштадт,  Зосимова, № 18/15, литер А",
                "189610, Санкт-Петербург, город Кронштадт, Посадская , № 4, литер А",
                "189630, Санкт-Петербург, город Колпино,  Металлургов, № 4, литер А",
                "189640, Санкт-Петербург, город Сестрорецк,  Борисова, № 9, корпус2, литер Х",
                "189640, Санкт-Петербург, город Сестрорецк, Дубковское шоссе, № 17, литер А",
                "189640, Санкт-Петербург, город Сестрорецк, Приморское шоссе, № 270, литер А",
        /*        "190000, г.Санкт-Петербург,  Декабристов, № 15, литер А",
                "190000, г.Санкт-Петербург,  Декабристов, № 33, литер Б",
                "190000, г.Санкт-Петербург,  Ефимова, № 1/4, литер А",
                "190000, г.Санкт-Петербург, Адмиралтейская набережная, № 10, литер А",
                "190000, г.Санкт-Петербург, Казанская , № 33/5, литер А",
                "190000, г.Санкт-Петербург, Курляндская , № 19-21, литер А",
                "190000, г.Санкт-Петербург, Курляндская , № 22-24, литер А",
                "190000, г.Санкт-Петербург, Нарвский проспект, № 9, литер А",
                "190000, г.Санкт-Петербург, Троицкий проспект, № 20/36, литер А",
                "190000, г.Санкт-Петербург, набережная канала Грибоедова, № 81, литер А",
                "190000, г.Санкт-Петербург, проспект Римского-Корсакова, № 37, литер А",
                "190008, г.Санкт-Петербург, проспект Римского-Корсакова, № 103, литер А",
                "190031, г.Санкт-Петербург, Московский проспект, № 4, литер А",
                "190031, г.Санкт-Петербург, Спасский переулок, № 3, литер А",
                "190068, г.Санкт-Петербург, Большая Подьяческая , № 16, литер А",
                "190121, г.Санкт-Петербург,  Декабристов, № 50, литер А",
                "190121, г.Санкт-Петербург,  Декабристов, № 57, литер А",
                "190121, г.Санкт-Петербург,  Союза Печатников, № 18-20, литер А",
                "190121, г.Санкт-Петербург, Витебская , № 5/6, литер А",
                "190121, г.Санкт-Петербург, Витебская , № 5/6, литер А",
                "190121, г.Санкт-Петербург, Мастерская , № 3, литер А",
                "191002, г.Санкт-Петербург,  Достоевского, № 9, литер А",
                "191002, г.Санкт-Петербург,  Рубинштейна, № 9/3, литер А",
                "191002, г.Санкт-Петербург, Большая Московская , № 1-3, литер А",
                "191002, г.Санкт-Петербург, Большая Московская , № 11, литер А",
                "191011, г.Санкт-Петербург, Итальянская , № 11, литер А",
                "191011, г.Санкт-Петербург, переулок Крылова, № 1, литер А",
                "191011, г.Санкт-Петербург, переулок Крылова, № 3, литер А",
                "191014, г.Санкт-Петербург,  Восстания, № 20/16, литер А",
                "191014, г.Санкт-Петербург,  Жуковского, № 43, литер А",
                "191023, г.Санкт-Петербург, Гороховая , № 32, литер А",
                "191023, г.Санкт-Петербург, Гороховая , № 44, литер А",
                "191023, г.Санкт-Петербург, Садовая , № 28-30, корпус36",
                "191023, г.Санкт-Петербург, набережная канала Грибоедова, № 30-32, литер Б",
                "191025, г.Санкт-Петербург,  Марата, № 16, литер А",
                "191025, г.Санкт-Петербург,  Марата, № 16, литер А",
                "191025, г.Санкт-Петербург, Владимирский проспект, № 20, литер Б",
                "191025, г.Санкт-Петербург, Невский проспект, № 88, литер А",
                "191025, г.Санкт-Петербург, набережная реки Фонтанки, № 40/68, литер А",
                "191040, г.Санкт-Петербург,  Марата, № 36-38, литер А",
                "191040, г.Санкт-Петербург, Кузнечный переулок, № 13, литер А",
                "191040, г.Санкт-Петербург, Лиговский проспект, № 139, литер А",
                "191040, г.Санкт-Петербург, Лиговский проспект, № 48, литер З",
                "191040, г.Санкт-Петербург, Лиговский проспект, № 72, литер А",
                "191040, г.Санкт-Петербург, Пушкинская , № 1/77, литер А",
                "191040, г.Санкт-Петербург, Пушкинская , № 11, литер А",
                "191104, г.Санкт-Петербург,  Белинского, № 5, литер А",
                "191104, г.Санкт-Петербург,  Маяковского, № 30, литер А",
                "191104, г.Санкт-Петербург,  Маяковского, № 32/11, литер А",
                "191104, г.Санкт-Петербург,  Чехова, № 4, литер А",
                "191104, г.Санкт-Петербург, Литейный проспект, № 58, литер А",
                "191104, г.Санкт-Петербург, Литейный проспект, № 61, литер А",
                "191119, г.Санкт-Петербург,  Достоевского, № 40-44, литер Д",
                "191119, г.Санкт-Петербург,  Константина Заслонова, № 6, литер А",
                "191123, г.Санкт-Петербург, Фурштатская , № 28, литер А",
                "191123, г.Санкт-Петербург, Шпалерная , № 38, литер А",
                "191126, г.Санкт-Петербург,  Достоевского, № 30, литер А",
                "191126, г.Санкт-Петербург,  Достоевского, № 36, литер А",
                "191126, г.Санкт-Петербург,  Марата, № 61, литер А",
                "191126, г.Санкт-Петербург,  Правды, № 20, литер А",
                "191126, г.Санкт-Петербург, Боровая , № 11-13, литер А",
                "191141, г.Санкт-Петербург, Литейный проспект, № 57, литер Б",
                "191180, г.Санкт-Петербург, Загородный проспект, № 27/21, литер А",
                "191180, г.Санкт-Петербург, Загородный проспект, № 29, литер А",
                "191186, г.Санкт-Петербург, Большая Конюшенная , № 3, литер А",
                "192007, г.Санкт-Петербург, Воронежская , № 59-61, литер А",
                "192007, г.Санкт-Петербург, Лиговский проспект, № 198, литер А",
                "192007, г.Санкт-Петербург, Тамбовская , № 6, литер А",
                "192007, г.Санкт-Петербург, Тамбовская , № 71-73, литер А",
                "192029, г.Санкт-Петербург,  Крупской, № 14, литер Б",
                "192071, г.Санкт-Петербург, проспект Славы, № 16, литер А",
                "192071, г.Санкт-Петербург, проспект Славы, № 40, корпус1, литер А",
                "192076, г.Санкт-Петербург, Рыбацкий проспект, № 49, корпус2, литер А",
                "192131, г.Санкт-Петербург,  Седова, № 94, литер А",
                "192131, г.Санкт-Петербург, Ивановская , № 12, литер А",
                "192131, г.Санкт-Петербург, Ивановская , № 18, литер Б",
                "192131, г.Санкт-Петербург, Ивановская , № 8/77, литер А",
                "192148, г.Санкт-Петербург,  Седова, № 19, литер А",
                "192148, г.Санкт-Петербург,  Ткачей, № 70, литер А",
                "192171, г.Санкт-Петербург,  Бабушкина, № 61, литер А",
                "192171, г.Санкт-Петербург,  Седова, № 71, литер А",
                "192171, г.Санкт-Петербург,  Седова, № 86, литер Д",
                "192171, г.Санкт-Петербург, проспект Обуховской Обороны, № 144, литер А",
                "192174, г.Санкт-Петербург,  Кибальчича, № 4, корпус3, литер Ф",
                "192212, г.Санкт-Петербург, Будапештская , № 19, корпус1, литер А",
                "192238, г.Санкт-Петербург, Бухарестская , № 72, корпус1, литер А",
                "192238, г.Санкт-Петербург, Бухарестская , № 94, корпус1, литер А",
                "192239, г.Санкт-Петербург, проспект Славы, № 21, литер А",
                "192242, г.Санкт-Петербург,  Турку, № 2, корпус3, литер А",
                "192242, г.Санкт-Петербург, Белградская , № 8, корпус1, литер А",
                "192242, г.Санкт-Петербург, Будапештская , № 8, корпус5, литер А",
                "192281, г.Санкт-Петербург,  Ярослава Гашека, № 12/100, литер А",
                "192281, г.Санкт-Петербург, Купчинская , № 6/4, корпус1, литер А",
                "192283, г.Санкт-Петербург,  Олеко Дундича, № 35, корпус1, литер А",
                "192283, г.Санкт-Петербург,  Ярослава Гашека, № 10/85, литер А",
                "192283, г.Санкт-Петербург, Будапештская , № 110/23, литер А",
                "192283, г.Санкт-Петербург, Купчинская , № 33, литер А",
                "192284, г.Санкт-Петербург, Будапештская , № 86, корпус1, литер А",
                "192284, г.Санкт-Петербург, Бухарестская , № 122, корпус1, литер А",
                "192284, г.Санкт-Петербург, Загребский бульвар, № 19, корпус1, литер А",
                "192286, г.Санкт-Петербург,  Димитрова, № 29, корпус1, литер А",
                "192286, г.Санкт-Петербург, Дунайский проспект, № 53, корпус2, литер А",
                "192288, г.Санкт-Петербург, Бухарестская , № 114, корпус1, литер А",
                "192288, г.Санкт-Петербург, Дунайский проспект, № 58, корпус1, литер А",
                "192288, г.Санкт-Петербург, Моравский переулок, № 7, корпус1, литер А",
                "192289, г.Санкт-Петербург, Малая Карпатская , № 21, литер А",
                "192289, г.Санкт-Петербург, Малая Карпатская , № 9, корпус1, литер А",
                "193012, г.Санкт-Петербург, Запорожская , № 21, литер А",
                "193012, г.Санкт-Петербург, проспект Обуховской Обороны, № 108, корпус2, литер П",
                "193019, г.Санкт-Петербург, проспект Обуховской Обороны, № 33, литер А",
                "193024, г.Санкт-Петербург, 2-я Советская , № 27/2, литер А",
                "193024, г.Санкт-Петербург, Невский проспект, № 129, литер А",
                "193024, г.Санкт-Петербург, Невский проспект, № 139, литер Б",
                "193024, г.Санкт-Петербург, Невский проспект, № 154, литер А",
                "193029, г.Санкт-Петербург,  Ольминского, № 2/83, литер А",
                "193036, г.Санкт-Петербург, 6-я Советская , № 21/2, литер А",
                "193036, г.Санкт-Петербург, Гончарная , № 21, литер А",
                "193036, г.Санкт-Петербург, Невский проспект, № 124, литер А",
                "193036, г.Санкт-Петербург, Невский проспект, № 136, литер А",
                "193040, г.Санкт-Петербург, Кузнечный переулок, № 17/2, литер А",
                "193076, г.Санкт-Петербург, Рыбацкий проспект, № 51, корпус1, литер А",
                "193076, г.Санкт-Петербург, Шлиссельбургский проспект, № 13, литер А",
                "193076, г.Санкт-Петербург, Шлиссельбургский проспект, № 2, корпус1, литер А",
                "193076, г.Санкт-Петербург, Шлиссельбургский проспект, № 2, корпус2, литер А",
                "193079, г.Санкт-Петербург, Народная , № 6, литер А",
                "193079, г.Санкт-Петербург, Октябрьская набережная, № 80, корпус1, литер Е",
                "193079, г.Санкт-Петербург, Октябрьская набережная, № 94, корпус4, литер А",
                "193124, г.Санкт-Петербург, Суворовский проспект, № 65, литер Ч",
                "193124, г.Санкт-Петербург, Суворовский проспект, № 67, литер А",
                "193131, г.Санкт-Петербург,  Седова, № 83/9, литер А",
                "193131, г.Санкт-Петербург, Ивановская , № 24, литер А",
                "193144, г.Санкт-Петербург, 10-я Советская , № 9, литер А",
                "193144, г.Санкт-Петербург, 8-я Советская , № 57, литер А",
                "193144, г.Санкт-Петербург, 9-я Советская , № 39/24, литер А",
                "193144, г.Санкт-Петербург, Суворовский проспект, № 43-45, литер А",
                "193144, г.Санкт-Петербург, проспект Бакунина, № 29, литер А",
                "193148, г.Санкт-Петербург,  Невзоровой, № 6, литер А",
                "193148, г.Санкт-Петербург,  Седова, № 21, литер А",
                "193167, г.Санкт-Петербург, Херсонская , № 10, литер А",
                "193168, г.Санкт-Петербург,  Антонова-Овсеенко, № 21, литер Я",
                "193168, г.Санкт-Петербург, Искровский проспект, № 15, корпус2, литер Л",
                "193171, г.Санкт-Петербург,  Полярников, № 15, литер А",
                "193171, г.Санкт-Петербург,  Седова, № 80, литер А",
                "193171, г.Санкт-Петербург, проспект Обуховской Обороны, № 143, литер А",
                "193177, г.Санкт-Петербург,  Дмитрия Устинова, № 8, литер А",
                "193177, г.Санкт-Петербург, Шлиссельбургский проспект, № 26, корпус2, литер А",
                "193177, г.Санкт-Петербург, Шлиссельбургский проспект, № 41, литер А",
                "193177, г.Санкт-Петербург, Шлиссельбургский проспект, № 47, корпус1, литер А",
                "193230, г.Санкт-Петербург,  Дыбенко, № 13, корпус1, литер Я",
                "193230, г.Санкт-Петербург,  Дыбенко, № 9, корпус1, литер Щ",
                "193230, г.Санкт-Петербург,  Крыленко, № 21, корпус1, литер С1",
                "193230, г.Санкт-Петербург, Искровский проспект, № 31, литер Э",
                "193231, г.Санкт-Петербург,  Бадаева, № 9, литер А",
                "193231, г.Санкт-Петербург,  Джона Рида, № 6, литер А",
                "193231, г.Санкт-Петербург,  Джона Рида, № 7, корпус1, литер А",
                "193231, г.Санкт-Петербург,  Коллонтай, № 20, литер А",
                "193231, г.Санкт-Петербург,  Коллонтай, № 23, корпус2, литер А",
                "193231, г.Санкт-Петербург, Клочков переулок, № 4, корпус1, литер А",
                "193231, г.Санкт-Петербург, Клочков переулок, № 8, литер А",
                "193231, г.Санкт-Петербург, Товарищеский проспект, № 1, корпус1, литер Н",
                "193231, г.Санкт-Петербург, проспект Пятилеток, № 19, литер А",
                "193231, г.Санкт-Петербург, проспект Солидарности, № 21, корпус3, литер А",
                "193232, г.Санкт-Петербург, проспект Большевиков, № 35, корпус3, литер Д",
                "193312, г.Санкт-Петербург,  Коллонтай, № 30, литер А",
                "193312, г.Санкт-Петербург,  Подвойского, № 34, корпус1, литер А",
                "193312, г.Санкт-Петербург,  Подвойского, № 48, корпус2, литер Н",
                "193312, г.Санкт-Петербург,  Подвойского, № 50, корпус2, литер М",
                "193312, г.Санкт-Петербург, проспект Солидарности, № 10, корпус1, литер А",
                "193312, г.Санкт-Петербург, проспект Солидарности, № 12, корпус2, литер И",
                "193313, г.Санкт-Петербург,  Коллонтай, № 21, корпус1, литер Б",
                "193313, г.Санкт-Петербург,  Коллонтай, № 3, литер А",
                "193313, г.Санкт-Петербург,  Подвойского, № 15, литер А",
                "193313, г.Санкт-Петербург, Искровский проспект, № 3, корпус2, литер Д",
                "193315, г.Санкт-Петербург, Народная , № 43, литер Г",
                "193315, г.Санкт-Петербург, Народная , № 73, литер А",
                "193315, г.Санкт-Петербург, Народная , № 80, литер Б",
                "193315, г.Санкт-Петербург, Народная , № 81, литер А",
                "193315, г.Санкт-Петербург, Народная , № 87, литер Р",
                "193315, г.Санкт-Петербург, Народная , № 98, литер О",
                "193318, г.Санкт-Петербург,  Коллонтай, № 24, корпус2, литер А",
                "193318, г.Санкт-Петербург,  Коллонтай, № 32, корпус2, литер А",
                "193318, г.Санкт-Петербург,  Кржижановского, № 5, корпус2, литер А",
                "193318, г.Санкт-Петербург,  Латышских Стрелков, № 5, корпус1, литер А",
                "193318, г.Санкт-Петербург,  Чудновского, № 2/11, литер А",
                "193318, г.Санкт-Петербург,  Чудновского, № 6, корпус1, литер А",
                "193318, г.Санкт-Петербург,  Чудновского, № 6, корпус2, литер А",
                "193318, г.Санкт-Петербург,  Чудновского, № 8, корпус1, литер А",
                "193318, г.Санкт-Петербург, Российский проспект, № 14, литер А",
                "194017, г.Санкт-Петербург, Гаврская , № 4, литер А",
                "194017, г.Санкт-Петербург, Дрезденская , № 4, литер Б",
                "194017, г.Санкт-Петербург, Енотаевская , № 16, литер Б",
                "194021, г.Санкт-Петербург, 2-й Муринский проспект, № 30, литер А",
                "194021, г.Санкт-Петербург, Институтский проспект, № 25, литер А",
                "194021, г.Санкт-Петербург, проспект Тореза, № 30, литер З",
                "194037, г.Санкт-Петербург, Сегалева , № 1/3, литер А",
                "194037, г.Санкт-Петербург, Шувалово, Софийская , № 10, литер А",
                "194100, г.Санкт-Петербург, Большой Сампсониевский проспект, № 82, литер А",
                "194100, г.Санкт-Петербург, Кантемировская , № 25, литер А",
                "194100, г.Санкт-Петербург, Литовская , № 8, литер А",
                "194156, г.Санкт-Петербург,  Орбели, № 11, литер А",
                "194223, г.Санкт-Петербург, 2-й Муринский проспект, № 14, литер О",
                "194223, г.Санкт-Петербург, Светлановский проспект, № 35, литер А",
                "194223, г.Санкт-Петербург, проспект Тореза, № 39, корпус1, литер А",
                "194223, г.Санкт-Петербург, проспект Тореза, № 40, корпус1, литер А",
                "194291, г.Санкт-Петербург,  Сантьяго-де-Куба, № 10, литер А",
                "194292, г.Санкт-Петербург, проспект Культуры, № 22, корпус1, литер А",
                "194295, г.Санкт-Петербург, проспект Художников, № 27, корпус1, литер А",
                "194295, г.Санкт-Петербург, проспект Художников, № 9, корпус2, литер А",
                "194352, г.Санкт-Петербург, Придорожная аллея, № 15, литер А",
                "194352, г.Санкт-Петербург, Придорожная аллея, № 17, литер А",
                "194352, г.Санкт-Петербург, Придорожная аллея, № 19, литер А",
                "194352, г.Санкт-Петербург, проспект Просвещения, № 54, литер А",
                "194352, г.Санкт-Петербург, проспект Художников, № 34/12, литер А",
                "194352, г.Санкт-Петербург, проспект Художников, № 39, корпус1, литер А",
                "194354, г.Санкт-Петербург,  Сикейроса, № 12, литер Б",
                "194354, г.Санкт-Петербург, Северный проспект, № 6, корпус1, литер А",
                "194354, г.Санкт-Петербург, проспект Художников, № 14, литер А",
                "194354, г.Санкт-Петербург, проспект Энгельса, № 111, корпус1, литер А",
                "194354, г.Санкт-Петербург, проспект Энгельса, № 120, литер А",
                "194355, г.Санкт-Петербург,  Жени Егоровой, № 3, корпус1, литер А",
                "194355, г.Санкт-Петербург,  Композиторов, № 15, литер А",
                "194355, г.Санкт-Петербург,  Композиторов, № 26/3, литер А",
                "194355, г.Санкт-Петербург,  Композиторов, № 26/3, литер А",
                "194355, г.Санкт-Петербург,  Композиторов, № 33/5, литер А",
                "194356, г.Санкт-Петербург,  Есенина, № 22, корпус1, литер А",
                "194356, г.Санкт-Петербург,  Есенина, № 26, корпус1, литер А",
                "194356, г.Санкт-Петербург,  Хошимина, № 11, корпус1, литер А",
                "194356, г.Санкт-Петербург,  Хошимина, № 11, корпус2, литер А",
                "194356, г.Санкт-Петербург,  Хошимина, № 5, корпус1, литер А",
                "194356, г.Санкт-Петербург, проспект Луначарского, № 7, корпус1, литер А",
                "194356, г.Санкт-Петербург, проспект Просвещения, № 19, литер А",
                "194356, г.Санкт-Петербург, проспект Просвещения, № 31, литер А",
                "194356, г.Санкт-Петербург, проспект Энгельса, № 128, литер А",
                "194356, г.Санкт-Петербург, проспект Энгельса, № 132, корпус1, литер А",
                "194356, г.Санкт-Петербург, проспект Энгельса, № 137, литер А",
                "194358, г.Санкт-Петербург, проспект Просвещения, № 28, литер А",
                "194358, г.Санкт-Петербург, проспект Просвещения, № 30, корпус1, литер А",
                "194358, г.Санкт-Петербург, проспект Энгельса, № 145, корпус1, литер А",
                "194358, г.Санкт-Петербург, проспект Энгельса, № 147, корпус1, литер Д",
                "195009, г.Санкт-Петербург,  Академика Лебедева, № 14/2, литер А",
                "195009, г.Санкт-Петербург,  Комсомола, № 10, литер А",
                "195009, г.Санкт-Петербург,  Комсомола, № 47, литер А",
                "195027, г.Санкт-Петербург, Красногвардейская площадь, № 6, литер А",
                "195030, г.Санкт-Петербург,  Коммуны, № 28, корпус3, литер А",
                "195030, г.Санкт-Петербург,  Осипенко, № 5, корпус1, литер А",
                "195043, г.Санкт-Петербург, Ржевская площадь, № 1, литер А",
                "195043, г.Санкт-Петербург, Челябинская , № 51, литер А",
                "195067, г.Санкт-Петербург, Пискаревский проспект, № 28, литер А",
                "195067, г.Санкт-Петербург, Пискаревский проспект, № 35, литер А",
                "195067, г.Санкт-Петербург, проспект Непокоренных, № 63, литер К38",
                "195197, г.Санкт-Петербург, Замшина , № 28, литер А",
                "195197, г.Санкт-Петербург, Кондратьевский проспект, № 40, корпус12, литер А",
                "195197, г.Санкт-Петербург, проспект Металлистов, № 99, литер А",
                "195213, г.Санкт-Петербург, Заневский проспект, № 38, литер А",
                "195220, г.Санкт-Петербург, Гражданский проспект, № 36, литер А",
                "195220, г.Санкт-Петербург, проспект Непокоренных, № 13, корпус1, литер А",
                "195248, г.Санкт-Петербург, проспект Энергетиков, № 30, корпус1, литер А",
                "195248, г.Санкт-Петербург, проспект Энергетиков, № 35, корпус1, литер А",
                "195248, г.Санкт-Петербург, шоссе Революции, № 31, литер А",
                "195248, г.Санкт-Петербург, шоссе Революции, № 33, корпус1, литер А",
                "195252, г.Санкт-Петербург,  Карпинского, № 31, корпус1, литер А",
                "195252, г.Санкт-Петербург,  Карпинского, № 38, корпус1, литер А",
                "195252, г.Санкт-Петербург,  Карпинского, № 38, корпус3, литер А",
                "195252, г.Санкт-Петербург,  Софьи Ковалевской, № 11, корпус1, литер А",
                "195252, г.Санкт-Петербург, Северный проспект, № 89, корпус1, литер А",
                "195252, г.Санкт-Петербург, проспект Науки, № 14, корпус2, литер А",
                "195253, г.Санкт-Петербург, проспект Маршала Блюхера, № 57, корпус1, литер А",
                "195253, г.Санкт-Петербург, проспект Энергетиков, № 48, литер А",
                "195257, г.Санкт-Петербург, Гражданский проспект, № 92, корпус1, литер А",
                "195257, г.Санкт-Петербург, Светлановский проспект, № 81/21, литер А",
                "195257, г.Санкт-Петербург, Северный проспект, № 61, корпус3, литер А",
                "195265, г.Санкт-Петербург,  Черкасова, № 10, корпус1, литер А",
                "195267, г.Санкт-Петербург, проспект Просвещения, № 84, корпус1, литер А",
                "195267, г.Санкт-Петербург, проспект Просвещения, № 87, корпус1, литер А",
                "195269, г.Санкт-Петербург,  Ольги Форш, № 7, корпус2, литер А",
                "195271, г.Санкт-Петербург, Замшина , № 31, литер А",
                "195273, г.Санкт-Петербург,  Карпинского, № 9, корпус1, литер А",
                "195274, г.Санкт-Петербург,  Демьяна Бедного, № 1, корпус2, литер А",
                "195274, г.Санкт-Петербург, проспект Культуры, № 11, корпус1, литер А",
                "195274, г.Санкт-Петербург, проспект Культуры, № 21, корпус1, литер А",
                "195275, г.Санкт-Петербург, проспект Энергетиков, № 68, литер А",
                "195276, г.Санкт-Петербург, Суздальский проспект, № 57, литер А",
                "195279, г.Санкт-Петербург, Индустриальный проспект, № 31, литер А",
                "195279, г.Санкт-Петербург, Индустриальный проспект, № 35, корпус2, литер А",
                "195279, г.Санкт-Петербург, проспект Ударников, № 27, корпус2, литер А",
                "195279, г.Санкт-Петербург, проспект Ударников, № 30, корпус1, литер А",
                "195279, г.Санкт-Петербург, проспект Ударников, № 32, корпус1, литер А",
                "195279, г.Санкт-Петербург, проспект Ударников, № 42, литер А",
                "195279, г.Санкт-Петербург, проспект Энтузиастов, № 18, корпус2, литер А",
                "195279, г.Санкт-Петербург, проспект Энтузиастов, № 28, корпус3, литер А",
                "195279, г.Санкт-Петербург, проспект Энтузиастов, № 30, корпус2, литер А",
                "195279, г.Санкт-Петербург, проспект Энтузиастов, № 50, литер Б",
                "195296, г.Санкт-Петербург, Белорусская , № 12, корпус1, литер А",
                "195298, г.Санкт-Петербург, Ленская , № 10, корпус1, литер А",
                "195298, г.Санкт-Петербург, Ленская , № 16, корпус2, литер А",
                "195298, г.Санкт-Петербург, Ленская , № 17, корпус1, литер А",
                "195298, г.Санкт-Петербург, проспект Наставников, № 12, литер А",
                "195298, г.Санкт-Петербург, проспект Наставников, № 12, литер А",
                "195298, г.Санкт-Петербург, проспект Наставников, № 15, корпус3, литер А",
                "195426, г.Санкт-Петербург, Ленская , № 11, корпус1, литер А",
                "195426, г.Санкт-Петербург, Ленская , № 2, корпус1, литер А",
                "195426, г.Санкт-Петербург, Ленская , № 8, корпус1, литер А",
                "195426, г.Санкт-Петербург, проспект Косыгина, № 21, корпус1, литер А",
                "195426, г.Санкт-Петербург, проспект Косыгина, № 25, корпус1, литер А",
                "196066, г.Санкт-Петербург, Московский проспект, № 224, литер Б",
                "196084, г.Санкт-Петербург, Заозерная , № 4, литер А",
                "196105, г.Санкт-Петербург, Московский проспект, № 168, литер А",
                "196128, г.Санкт-Петербург, Варшавская , № 34, литер А",
                "196128, г.Санкт-Петербург, Новоизмайловский проспект, № 4, литер А",
                "196135, г.Санкт-Петербург, Московский проспект, № 200, литер А",
                "196143, г.Санкт-Петербург, площадь Победы, литер А, (сооружение 1)",
                "196158, г.Санкт-Петербург, Дунайский проспект, № 5, литер А",
                "196158, г.Санкт-Петербург, Звездная , № 3, корпус1, литер А",
                "196210, г.Санкт-Петербург,  Пилотов, № 35, литер Г",
                "196233, г.Санкт-Петербург, Витебский проспект, № 108, литер А",
                "196600, Санкт-Петербург, город Пушкин, Академический проспект, № 31, литер А",
                "196600, Санкт-Петербург, город Пушкин, Детскосельский бульвар, № 5, литер А",
                "196600, Санкт-Петербург, город Пушкин, Пушкинская , № 34/31, литер А",
                "196641, Санкт-Петербург, поселок Металлострой, Полевая , № 2/30, литер А",
                "196641, Санкт-Петербург, поселок Металлострой, Центральная , № 4а, литер А",
                "196643, Санкт-Петербург, поселок Понтонный,  Александра Товпеко, № 32, литер А",
                "196644, Санкт-Петербург, поселок Саперный, Дорожная , № 7, литер А",
                "196653, Санкт-Петербург, город Колпино,  Веры Слуцкой, № 32, литер А",
                "196653, Санкт-Петербург, город Колпино, Пролетарская , № 38, литер А",
                "196655, Санкт-Петербург, город Колпино,  Ижорского Батальона, № 23, литер А",
                "196655, Санкт-Петербург, город Колпино,  Труда, № 1/7, литер А",
                "196655, Санкт-Петербург, город Колпино, Московская , № 11, литер А",
                "196655, Санкт-Петербург, город Колпино, Тверская , № 60, литер А",
                "196657, Санкт-Петербург, город Колпино, Заводской проспект, № 8, литер А",
                "197022, г.Санкт-Петербург,  Профессора Попова, № 41/5, литер Б",
                "197022, г.Санкт-Петербург, Каменноостровский проспект, № 41, литер А",
                "197046, г.Санкт-Петербург,  Куйбышева, № 29, литер А",
                "197046, г.Санкт-Петербург,  Чапаева, № 11/4, литер А",
                "197068, г.Санкт-Петербург, Введенская , № 19, литер А",
                "197101, г.Санкт-Петербург,  Воскова, № 15-17, литер А",
                "197101, г.Санкт-Петербург, Большой проспект П.С., № 55/6, литер А",
                "197101, г.Санкт-Петербург, Большой проспект П.С., № 69, литер А",
                "197101, г.Санкт-Петербург, Кронверкская , № 15, литер А",
                "197101, г.Санкт-Петербург, Кронверкская , № 17/1, литер А",
                "197110, г.Санкт-Петербург, Большая Зеленина , № 19, литер А",
                "197110, г.Санкт-Петербург, Большая Зеленина , № 28, литер А",
                "197110, г.Санкт-Петербург, Малый проспект П.С., № 54-56, литер А",
                "197110, г.Санкт-Петербург, Пионерская , № 31, литер А",
                "197136, г.Санкт-Петербург,  Всеволода Вишневского, № 10, литер А",
                "197136, г.Санкт-Петербург,  Ленина, № 27, литер А",
                "197183, г.Санкт-Петербург, Сестрорецкая , № 2, литер А",
                "197198, г.Санкт-Петербург,  Воскова, № 8/5, литер А",
                "197198, г.Санкт-Петербург, Большой проспект П.С., № 2/1, литер А",
                "197198, г.Санкт-Петербург, Большой проспект П.С., № 45, литер А",
                "197198, г.Санкт-Петербург, Введенская , № 5/13, литер А",
                "197198, г.Санкт-Петербург, Зверинская , № 42, литер А",
                "197198, г.Санкт-Петербург, Малый проспект П.С., № 5, литер Б",
                "197198, г.Санкт-Петербург, Ораниенбаумская , № 13, литер А",
                "197341, г.Санкт-Петербург, аллея Котельникова, № 2, литер А",
                "197341, г.Санкт-Петербург, проспект Испытателей, № 13, литер А",
                "197341, г.Санкт-Петербург, проспект Испытателей, № 15, корпус1, литер А",
                "197341, г.Санкт-Петербург, проспект Королева, № 19, литер А",
                "197342, г.Санкт-Петербург, Ланское шоссе, № 59, литер А",
                "197342, г.Санкт-Петербург, Новосибирская , № 18/5, литер А",
                "197349, г.Санкт-Петербург, Новоколомяжский проспект, № 12, корпус2, литер А",
                "197371, г.Санкт-Петербург,  Уточкина, № 6, корпус3, литер А",
                "197372, г.Санкт-Петербург, Беговая , № 11, литер А",
                "197372, г.Санкт-Петербург, Богатырский проспект, № 57, корпус3, литер А",
                "197372, г.Санкт-Петербург, Стародеревенская , № 29, литер А",
                "197372, г.Санкт-Петербург, Туристская , № 38, литер А",
                "197372, г.Санкт-Петербург, Яхтенная , № 35, литер А",
                "197372, г.Санкт-Петербург, проспект Авиаконструкторов, № 13, литер А",
                "197373, г.Санкт-Петербург,  Шаврова, № 9, литер А",
                "197373, г.Санкт-Петербург, Камышовая , № 9, корпус1, литер А",
                "197373, г.Санкт-Петербург, Комендантский проспект, № 40, корпус1, литер А",
                "197373, г.Санкт-Петербург, Планерная , № 45, корпус1, литер А",
                "197373, г.Санкт-Петербург, проспект Авиаконструкторов, № 25, литер А",
                "197373, г.Санкт-Петербург, проспект Авиаконструкторов, № 44, корпус1, литер А",
                "197374, г.Санкт-Петербург, Стародеревенская , № 6, корпус3, литер А",
                "197760, Санкт-Петербург, город Кронштадт, Посадская , № 1/82, литер А",
                "198013, г.Санкт-Петербург, Бронницкая , № 31, литер А",
                "198013, г.Санкт-Петербург, Верейская , № 1/62, литер В",
                "198013, г.Санкт-Петербург, Можайская , № 37-39, литер А",
                "198013, г.Санкт-Петербург, Московский проспект, № 42/27, литер А",
                "198013, г.Санкт-Петербург, Подольская , № 39, литер А",
                "198013, г.Санкт-Петербург, Рузовская , № 25, литер А",
                "198052, г.Санкт-Петербург, набережная Обводного канала, № 147-149, литер К",
                "198103, г.Санкт-Петербург, Дерптский переулок, № 15/11, литер А",
                "198103, г.Санкт-Петербург, Курляндская , № 32, литер А",
                "198188, г.Санкт-Петербург,  Васи Алексеева, № 6, литер А",
                "198205, г.Санкт-Петербург, Авангардная , № 39, корпус1, литер А",
                "198205, г.Санкт-Петербург, Старо-Паново, Пролетарская , № 10, литер А",
                "198205, г.Санкт-Петербург, проспект Ветеранов, № 115, литер А",
                "198206, г.Санкт-Петербург,  Летчика Пилютова, № 6, корпус2, литер Д",
                "198207, г.Санкт-Петербург, Ленинский проспект, № 118, корпус1, литер А",
                "198259, г.Санкт-Петербург,  Здоровцева, № 27, корпус1, литер А",
                "198261, г.Санкт-Петербург, проспект Ветеранов, № 110, литер А",
                "198264, Санкт-Петербург, город Красное Село,  Спирина, № 2, корпус1, литер А",
                "198264, г.Санкт-Петербург, проспект Ветеранов, № 141, корпус1, литер А",
                "198264, г.Санкт-Петербург, проспект Ветеранов, № 149, литер А",
                "198302, г.Санкт-Петербург,  Маршала Казакова, № 28, корпус3, литер А",
                "198320, Санкт-Петербург, город Красное Село, Стрельнинское шоссе, № 6, корпус3, литер А",
                "198323, г.Санкт-Петербург, Горелово,  Коммунаров, № 116, корпус2, литер А",
                "198323, г.Санкт-Петербург, Горелово, Школьная , № 45, литер А",
                "198326, г.Санкт-Петербург, Торики,  Политрука Пасечника, № 9, литер А",
                "198328, г.Санкт-Петербург,  Маршала Захарова, № 22, корпус1, литер А",
                "198328, г.Санкт-Петербург,  Маршала Захарова, № 33, корпус1, литер А",
                "198328, г.Санкт-Петербург, Брестский бульвар, № 17, литер А",
                "198328, г.Санкт-Петербург, Петергофское шоссе, № 11/21, литер А",
                "198328, г.Санкт-Петербург, Петергофское шоссе, № 15, корпус2, литер А",
                "198329, г.Санкт-Петербург,  Отважных, № 4, литер А",
                "198329, г.Санкт-Петербург,  Партизана Германа, № 11, литер А",
                "198329, г.Санкт-Петербург, проспект Ветеранов, № 129, корпус2, литер А",
                "198330, г.Санкт-Петербург,  Десантников, № 12, корпус1, литер А",
                "198330, г.Санкт-Петербург,  Десантников, № 34, литер А",
                "198330, г.Санкт-Петербург,  Котина, № 7, корпус1, литер А",
                "198330, г.Санкт-Петербург,  Маршала Казакова, № 24, корпус2, литер А",
                "198330, г.Санкт-Петербург, Ленинский проспект, № 100, корпус2, литер А",
                "198330, г.Санкт-Петербург, Ленинский проспект, № 96, корпус2, литер А",
                "198330, г.Санкт-Петербург, Петергофское шоссе, № 1, корпус1, литер А",
                "198332, г.Санкт-Петербург,  Маршала Казакова, № 28, корпус1, литер А",
                "198332, г.Санкт-Петербург, Ленинский проспект, № 92, корпус1, литер А",
                "198332, г.Санкт-Петербург, Ленинский проспект, № 92, корпус3, литер А",
                "198332, г.Санкт-Петербург, проспект Маршала Жукова, № 37, корпус1, литер А",
                "198334, г.Санкт-Петербург,  Добровольцев, № 38, литер А",
                "198334, г.Санкт-Петербург,  Партизана Германа, № 37, литер А",
                "199004, г.Санкт-Петербург, 6-я линия В.О., № 31/29, литер Б",
                "199004, г.Санкт-Петербург, Средний проспект В.О., № 32, литер А",
                "199026, г.Санкт-Петербург, 26-я линия В.О., № 9, литер А",
                "199026, г.Санкт-Петербург, Большой проспект В.О., № 56, литер А",
                "199026, г.Санкт-Петербург, Большой проспект В.О., № 62, литер А",
                "199034, г.Санкт-Петербург, 13-я линия В.О., № 10, литер А",
                "199034, г.Санкт-Петербург, 13-я линия В.О., № 2/19, литер А",
                "199048, г.Санкт-Петербург, 12-я линия В.О., № 53, литер А",
                "199048, г.Санкт-Петербург, 17-я линия В.О., № 70/12, литер А",
                "199053, г.Санкт-Петербург, 1-я линия В.О., № 26, литер А",
                "199053, г.Санкт-Петербург, 3-я линия В.О., № 42, литер А",
                "199057, г.Санкт-Петербург, Железноводская , № 29, литер А",
                "199057, г.Санкт-Петербург, Наличная , № 40, корпус1, литер А",
                "199057, г.Санкт-Петербург, переулок Каховского, № 3, литер А",
                "199151, г.Санкт-Петербург, Гаванская , № 41, литер А",
                "199155, г.Санкт-Петербург,  Кораблестроителей, № 29, корпус4, литер А",
                "199155, г.Санкт-Петербург, Железноводская , № 42, литер А",
                "199155, г.Санкт-Петербург, проспект КИМа, № 4, литер А",
                "199161, г.Санкт-Петербург, Малый проспект В.О., № 3/60, литер А",
                "199178, г.Санкт-Петербург, 10-я линия В.О., № 15б, литер А",
                "199178, г.Санкт-Петербург, 11-я линия В.О., № 14/39, литер А",
                "199178, г.Санкт-Петербург, Средний проспект В.О., № 48/27, литер А",
                "199178, г.Санкт-Петербург, Средний проспект В.О., № 49, литер А",
                "199178, г.Санкт-Петербург, Средний проспект В.О., № 68, литер Б",
                "199226, г.Санкт-Петербург,  Кораблестроителей, № 16, корпус1, литер А",
                "199226, г.Санкт-Петербург,  Кораблестроителей, № 19, корпус1, литер А",
                "199226, г.Санкт-Петербург, Морская набережная, № 15, литер А",
                "199226, г.Санкт-Петербург, Морская набережная, № 15, литер А",
                "199226, г.Санкт-Петербург, Наличная , № 36, корпус1, литер А",
                "199226, г.Санкт-Петербург, Наличная , № 45, корпус1, литер А",
                "199406, г.Санкт-Петербург,  Шевченко, № 37, литер А",
                "Санкт-Петербург, город Зеленогорск, Комсомольская , № 13, литер А",
                "Санкт-Петербург, город Колпино,  Анисимова, № 10",
                "Санкт-Петербург, город Колпино,  Братьев Радченко, № 19",
                "Санкт-Петербург, город Колпино,  Веры Слуцкой, № 19, литер А",
                "Санкт-Петербург, город Колпино,  Веры Слуцкой, № 19, литер А",
                "Санкт-Петербург, город Колпино,  Веры Слуцкой, № 42",
                "Санкт-Петербург, город Колпино,  Веры Слуцкой, № 48",
                "Санкт-Петербург, город Колпино,  Веры Слуцкой, № 87, литер А",
                "Санкт-Петербург, город Колпино,  Ижорского Батальона, № 16",
                "Санкт-Петербург, город Колпино,  Ижорского Батальона, № 18",
                "Санкт-Петербург, город Колпино,  Ижорского Батальона, № 7, литер А",
                "Санкт-Петербург, город Колпино,  Металлургов, № 9",
                "Санкт-Петербург, город Колпино, Заводской проспект, № 14",
                "Санкт-Петербург, город Колпино, Загородная , № 31",
                "Санкт-Петербург, город Колпино, Октябрьская , № 61, литер А",
                "Санкт-Петербург, город Колпино, Октябрьская , № 67",
                "Санкт-Петербург, город Колпино, Октябрьская , № 69",
                "Санкт-Петербург, город Колпино, Павловская , № 27, литер А",
                "Санкт-Петербург, город Колпино, Павловская , № 52, литер А",
                "Санкт-Петербург, город Колпино, Павловская , № 78",
                "Санкт-Петербург, город Колпино, Павловская , № 90, литер А",
                "Санкт-Петербург, город Колпино, Павловская , № 92",
                "Санкт-Петербург, город Колпино, Пролетарская , № 103",
                "Санкт-Петербург, город Колпино, Пролетарская , № 11, литер А",
                "Санкт-Петербург, город Колпино, Тверская , № 36/9, литер А",
                "Санкт-Петербург, город Колпино, Тверская , № 48",
                "Санкт-Петербург, город Колпино, бульвар Трудящихся, № 15, корпус2",
                "Санкт-Петербург, город Колпино, бульвар Трудящихся, № 18",
                "Санкт-Петербург, город Колпино, бульвар Трудящихся, № 24",
                "Санкт-Петербург, город Колпино, бульвар Трудящихся, № 36",
                "Санкт-Петербург, город Колпино, проспект Ленина, № 35",
                "Санкт-Петербург, город Колпино, проспект Ленина, № 47",
                "Санкт-Петербург, город Колпино, участок ж.д. 'ул.Урицкого - Комсомольский кан.', литер А",
                "Санкт-Петербург, город Красное Село, Красногородская , № 11, корпус1, литер А",
                "Санкт-Петербург, город Красное Село, Красногородская , № 7, корпус1, литер А",
                "Санкт-Петербург, город Красное Село, Красногородская , № 9, корпус1, литер А",
                "Санкт-Петербург, город Красное Село, Нагорная , № 42, литер А",
                "Санкт-Петербург, город Красное Село, Стрельнинское шоссе, № 4, корпус2",
                "Санкт-Петербург, город Кронштадт,  Восстания, № 72, литер А",
                "Санкт-Петербург, город Кронштадт, Кронштадтская , № 9, литер А",
                "Санкт-Петербург, город Павловск,  Толмачева, № 3",
                "Санкт-Петербург, город Петергоф,  Шахматова, № 16, литер А",
                "Санкт-Петербург, город Петергоф,  Шахматова, № 2, корпус1",
                "Санкт-Петербург, город Петергоф, Озерковая , № 45, литер А",
                "Санкт-Петербург, город Петергоф, Ропшинское шоссе, № 8",
                "Санкт-Петербург, город Петергоф, Суворовская , № 7, корпус5",
                "Санкт-Петербург, город Петергоф, Троицкая , № 26, литер А",
                "Санкт-Петербург, город Петергоф, Чебышевская , № 3, корпус1",
                "Санкт-Петербург, город Пушкин, Артиллерийская , № 6",
                "Санкт-Петербург, город Пушкин, Ленинградская , № 75, литер А",
                "Санкт-Петербург, город Сестрорецк, 3-я линия, № 5, литер А",
                "Санкт-Петербург, поселок Металлострой, Полевая , № 27",
                "Санкт-Петербург, поселок Парголово,  Первого Мая, № 107, корпус6, литер А",
                "Санкт-Петербург, поселок Песочный, Ленинградская , № 68, литер А",
                "Санкт-Петербург, поселок Стрельна, Вокзальная , № 4, литер А",
                "Санкт-Петербург, поселок Стрельна, Кропоткинская , № 6",
                "Санкт-Петербург, поселок Стрельна, Санкт-Петербургское шоссе, № 86, литер А",
                "Санкт-Петербург, поселок Шушары, Ленсоветовский, № 6",
                "Санкт-Петербург, поселок Шушары, Школьная , № 34",
                "г.Санкт-Петербург,  Академика Байкова, № 11, корпус1, литер А",
                "г.Санкт-Петербург,  Антонова-Овсеенко, № 19, корпус3, литер З",
                "г.Санкт-Петербург,  Антонова-Овсеенко, № 5, корпус2, литер И",
                "г.Санкт-Петербург,  Асафьева, № 10",
                "г.Санкт-Петербург,  Асафьева, № 6, корпус1, литер А",
                "г.Санкт-Петербург,  Бабушкина, № 115, корпус5, литер З",
                "г.Санкт-Петербург,  Бабушкина, № 133, литер М",
                "г.Санкт-Петербург,  Бабушкина, № 49, литер А",
                "г.Санкт-Петербург,  Белы Куна, № 15, корпус1",
                "г.Санкт-Петербург,  Белы Куна, № 15, корпус2, литер А",
                "г.Санкт-Петербург,  Беринга, № 23, корпус1, литер А",
                "г.Санкт-Петербург,  Брянцева, № 12, корпус1",
                "г.Санкт-Петербург,  Брянцева, № 2, корпус1",
                "г.Санкт-Петербург,  Бурцева, № 11",
                "г.Санкт-Петербург,  Вавиловых, № 14, литер А",
                "г.Санкт-Петербург,  Вавиловых, № 4, корпус1, литер В",
                "г.Санкт-Петербург,  Верности, № 14, корпус1, литер А",
                "г.Санкт-Петербург,  Верности, № 42",
                "г.Санкт-Петербург,  Верности, № 52",
                "г.Санкт-Петербург,  Генерала Симоняка, № 1, корпус2, литер А",
                "г.Санкт-Петербург,  Генерала Симоняка, № 10, литер А",
                "г.Санкт-Петербург,  Генерала Симоняка, № 25, литер А",
                "г.Санкт-Петербург,  Генерала Симоняка, № 3",
                "г.Санкт-Петербург,  Глинки, № 3-5-7, литер А",
                "г.Санкт-Петербург,  Декабристов, № 29, литер А",
                "г.Санкт-Петербург,  Демьяна Бедного, № 23, корпус2",
                "г.Санкт-Петербург,  Демьяна Бедного, № 30, корпус2",
                "г.Санкт-Петербург,  Десантников, № 20, корпус1, литер А",
                "г.Санкт-Петербург,  Десантников, № 24, литер А",
                "г.Санкт-Петербург,  Десантников, № 32, корпус3",
                "г.Санкт-Петербург,  Димитрова, № 11",
                "г.Санкт-Петербург,  Димитрова, № 13",
                "г.Санкт-Петербург,  Димитрова, № 13",
                "г.Санкт-Петербург,  Димитрова, № 24, корпус1",
                "г.Санкт-Петербург,  Димитрова, № 31",
                "г.Санкт-Петербург,  Дмитрия Устинова, № 4, корпус1, литер А",
                "г.Санкт-Петербург,  Доблести, № 26, корпус2",
                "г.Санкт-Петербург,  Добровольцев, № 42",
                "г.Санкт-Петербург,  Дыбенко, № 11, корпус2, литер А",
                "г.Санкт-Петербург,  Дыбенко, № 18, корпус2, литер А",
                "г.Санкт-Петербург,  Дыбенко, № 22, корпус3, литер Д",
                "г.Санкт-Петербург,  Дыбенко, № 22, корпус5, литер К",
                "г.Санкт-Петербург,  Дыбенко, № 23, корпус1, литер Ф",
                "г.Санкт-Петербург,  Дыбенко, № 23, корпус4, литер М",
                "г.Санкт-Петербург,  Дыбенко, № 36, корпус1, литер В",
                "г.Санкт-Петербург,  Евдокима Огнева, № 12, корпус1, литер З",
                "г.Санкт-Петербург,  Есенина, № 11, корпус1, литер Я",
                "г.Санкт-Петербург,  Есенина, № 14, корпус2",
                "г.Санкт-Петербург,  Зайцева, № 20",
                "г.Санкт-Петербург,  Зины Портновой, № 1/3, литер А",
                "г.Санкт-Петербург,  Зины Портновой, № 21, корпус2, литер А",
                "г.Санкт-Петербург,  Зины Портновой, № 21, корпус3",
                "г.Санкт-Петербург,  Зины Портновой, № 60, литер А",
                "г.Санкт-Петербург,  Зодчего Росси, № 1-3, литер А",
                "г.Санкт-Петербург,  Зои Космодемьянской, № 3, литер А",
                "г.Санкт-Петербург,  Ивана Фомина, № 5, корпус1",
                "г.Санкт-Петербург,  Ильюшина, № 2, литер А",
                "г.Санкт-Петербург,  Карпинского, № 14, литер А",
                "г.Санкт-Петербург,  Карпинского, № 28, корпус1",
                "г.Санкт-Петербург,  Карпинского, № 33, корпус1, литер А",
                "г.Санкт-Петербург,  Коллонтай, № 16, корпус3, литер А",
                "г.Санкт-Петербург,  Коллонтай, № 21, корпус1, литер А",
                "г.Санкт-Петербург,  Коллонтай, № 21, корпус4, литер Ю",
                "г.Санкт-Петербург,  Коллонтай, № 45, корпус2, литер И",
                "г.Санкт-Петербург,  Коллонтай, № 47, корпус5, литер А",
                "г.Санкт-Петербург,  Композиторов, № 33, корпус3",
                "г.Санкт-Петербург,  Кораблестроителей, № 23, корпус1, литер А",
                "г.Санкт-Петербург,  Костюшко, № 12, литер А",
                "г.Санкт-Петербург,  Костюшко, № 2, литер А",
                "г.Санкт-Петербург,  Костюшко, № 48, литер А",
                "г.Санкт-Петербург,  Кржижановского, № 17, корпус2, литер А",
                "г.Санкт-Петербург,  Кржижановского, № 7, литер А",
                "г.Санкт-Петербург,  Кржижановского, № 7, литер А",
                "г.Санкт-Петербург,  Крупской, № 35, литер Б",
                "г.Санкт-Петербург,  Крыленко, № 13, корпус2, литер Ж1",
                "г.Санкт-Петербург,  Крыленко, № 17, корпус2, литер З",
                "г.Санкт-Петербург,  Крыленко, № 19, корпус1, литер Щ",
                "г.Санкт-Петербург,  Крыленко, № 33, литер И",
                "г.Санкт-Петербург,  Крыленко, № 9, корпус1, литер Б",
                "г.Санкт-Петербург,  Лени Голикова, № 100",
                "г.Санкт-Петербург,  Лени Голикова, № 34",
                "г.Санкт-Петербург,  Лени Голикова, № 50",
                "г.Санкт-Петербург,  Ленсовета, № 22, литер А",
                "г.Санкт-Петербург,  Ленсовета, № 62, корпус2",
                "г.Санкт-Петербург,  Ленсовета, № 64, литер А",
                "г.Санкт-Петербург,  Ленсовета, № 95, литер А",
                "г.Санкт-Петербург,  Ленсовета, № 97, литер А",
                "г.Санкт-Петербург,  Ломоносова, № 20, литер А",
                "г.Санкт-Петербург,  Марата, № 15, литер А",
                "г.Санкт-Петербург,  Марата, № 56-58/29, литер А",
                "г.Санкт-Петербург,  Маршала Говорова, № 14, литер А",
                "г.Санкт-Петербург,  Маршала Говорова, № 6/5, литер А",
                "г.Санкт-Петербург,  Маршала Захарова, № 25, корпус1",
                "г.Санкт-Петербург,  Маршала Захарова, № 29, корпус3",
                "г.Санкт-Петербург,  Маршала Захарова, № 30, корпус1",
                "г.Санкт-Петербург,  Маршала Казакова, № 22, корпус1, литер А",
                "г.Санкт-Петербург,  Маршала Казакова, № 40, корпус1",
                "г.Санкт-Петербург,  Маршала Казакова, № 5, корпус1, литер А",
                "г.Санкт-Петербург,  Матроса Железняка, № 1",
                "г.Санкт-Петербург,  Матроса Железняка, № 3",
                "г.Санкт-Петербург,  Маяковского, № 23/6, литер А",
                "г.Санкт-Петербург,  Моисеенко, № 14",
                "г.Санкт-Петербург,  Нахимова, № 5, корпус3, литер А",
                "г.Санкт-Петербург,  Нахимова, № 6",
                "г.Санкт-Петербург,  Одоевского, № 33, литер А",
                "г.Санкт-Петербург,  Олеко Дундича, № 35, корпус3",
                "г.Санкт-Петербург,  Олеко Дундича, № 8, корпус1",
                "г.Санкт-Петербург,  Ольги Берггольц, № 29, корпус2, литер А",
                "г.Санкт-Петербург,  Орджоникидзе, № 15, литер А",
                "г.Санкт-Петербург,  Орджоникидзе, № 44, корпус4, литер А",
                "г.Санкт-Петербург,  Орджоникидзе, № 47, литер А",
                "г.Санкт-Петербург,  Осипенко, № 10, корпус1, литер А",
                "г.Санкт-Петербург,  Партизана Германа, № 10, корпус1, литер А",
                "г.Санкт-Петербург,  Партизана Германа, № 14/117, литер А",
                "г.Санкт-Петербург,  Передовиков, № 1/6",
                "г.Санкт-Петербург,  Передовиков, № 25, литер А",
                "г.Санкт-Петербург,  Передовиков, № 37, литер А",
                "г.Санкт-Петербург,  Писарева, № 5, литер А",
                "г.Санкт-Петербург,  Победы, № 9, литер А",
                "г.Санкт-Петербург,  Пограничника Гарькавого, № 16, корпус4",
                "г.Санкт-Петербург,  Подводника Кузьмина, № 1",
                "г.Санкт-Петербург,  Подводника Кузьмина, № 31",
                "г.Санкт-Петербург,  Подводника Кузьмина, № 9",
                "г.Санкт-Петербург,  Подвойского, № 14, корпус1, литер М",
                "г.Санкт-Петербург,  Подвойского, № 17, корпус1, литер А",
                "г.Санкт-Петербург,  Подвойского, № 24, корпус2, литер Ц",
                "г.Санкт-Петербург,  Подвойского, № 24, корпус2, литер Ц",
                "г.Санкт-Петербург,  Подвойского, № 35, корпус1, литер П",
                "г.Санкт-Петербург,  Подвойского, № 36, корпус1, литер А",
                "г.Санкт-Петербург,  Подвойского, № 38, литер А",
                "г.Санкт-Петербург,  Примакова, № 8",
                "г.Санкт-Петербург,  Профессора Попова, № 42",
                "г.Санкт-Петербург,  Решетникова, № 12, литер А",
                "г.Санкт-Петербург,  Розенштейна, № 1",
                "г.Санкт-Петербург,  Рубинштейна, № 23, литер А",
                "г.Санкт-Петербург,  Рубинштейна, № 26, литер А",
                "г.Санкт-Петербург,  Рубинштейна, № 40/11, литер А",
                "г.Санкт-Петербург,  Руднева, № 27, корпус1",
                "г.Санкт-Петербург,  Руднева, № 28, корпус1",
                "г.Санкт-Петербург,  Руставели, № 13, литер А",
                "г.Санкт-Петербург,  Руставели, № 30, литер А",
                "г.Санкт-Петербург,  Руставели, № 37, литер А",
                "г.Санкт-Петербург,  Руставели, № 68, литер А",
                "г.Санкт-Петербург,  Савушкина, № 115, корпус1, литер А",
                "г.Санкт-Петербург,  Савушкина, № 130, корпус1, литер А",
                "г.Санкт-Петербург,  Савушкина, № 6, литер А",
                "г.Санкт-Петербург,  Седова, № 109, литер Г2",
                "г.Санкт-Петербург,  Седова, № 31, литер А",
                "г.Санкт-Петербург,  Седова, № 67, литер А",
                "г.Санкт-Петербург,  Седова, № 76, литер В",
                "г.Санкт-Петербург,  Седова, № 91, корпус1, литер А",
                "г.Санкт-Петербург,  Сикейроса, № 15, корпус1",
                "г.Санкт-Петербург,  Сикейроса, № 21, корпус2",
                "г.Санкт-Петербург,  Солдата Корзуна, № 34, литер А",
                "г.Санкт-Петербург,  Солдата Корзуна, № 50",
                "г.Санкт-Петербург,  Софьи Ковалевской, № 4, литер А",
                "г.Санкт-Петербург,  Стойкости, № 16",
                "г.Санкт-Петербург,  Стойкости, № 18, корпус1",
                "г.Санкт-Петербург,  Стойкости, № 29, корпус2",
                "г.Санкт-Петербург,  Стойкости, № 37, литер А",
                "г.Санкт-Петербург,  Стойкости, № 4",
                "г.Санкт-Петербург,  Тамбасова, № 8, корпус2",
                "г.Санкт-Петербург,  Танкиста Хрустицкого, № 106, литер А",
                "г.Санкт-Петербург,  Танкиста Хрустицкого, № 33",
                "г.Санкт-Петербург,  Танкиста Хрустицкого, № 46, литер А",
                "г.Санкт-Петербург,  Тельмана, № 31, литер А",
                "г.Санкт-Петербург,  Тельмана, № 32, корпус1, литер В1",
                "г.Санкт-Петербург,  Тельмана, № 32, корпус2, литер Р",
                "г.Санкт-Петербург,  Тельмана, № 40, литер Я",
                "г.Санкт-Петербург,  Тельмана, № 50, литер Т",
                "г.Санкт-Петербург,  Типанова, № 19, литер А",
                "г.Санкт-Петербург,  Типанова, № 21, литер А",
                "г.Санкт-Петербург,  Типанова, № 32, корпус2",
                "г.Санкт-Петербург,  Ткачей, № 24, литер А",
                "г.Санкт-Петербург,  Трефолева, № 11, литер Л",
                "г.Санкт-Петербург,  Турку, № 12, корпус2, литер А",
                "г.Санкт-Петербург,  Турку, № 19, корпус2, литер А",
                "г.Санкт-Петербург,  Турку, № 20, корпус1",
                "г.Санкт-Петербург,  Турку, № 28, корпус2, литер А",
                "г.Санкт-Петербург,  Турку, № 9, корпус1, литер А",
                "г.Санкт-Петербург,  Турку, № 9, корпус2",
                "г.Санкт-Петербург,  Турку, № 9, корпус5",
                "г.Санкт-Петербург,  Уточкина, № 3, корпус2, литер А",
                "г.Санкт-Петербург,  Фрунзе, № 15, литер А",
                "г.Санкт-Петербург,  Фрунзе, № 6, литер А",
                "г.Санкт-Петербург,  Хошимина, № 10",
                "г.Санкт-Петербург,  Цимбалина, № 44, литер Б",
                "г.Санкт-Петербург,  Цимбалина, № 52, литер Д",
                "г.Санкт-Петербург,  Чекистов, № 20, литер А",
                "г.Санкт-Петербург,  Черкасова, № 4, корпус1",
                "г.Санкт-Петербург,  Чехова, № 12-16, литер А",
                "г.Санкт-Петербург,  Чудновского, № 13, литер А",
                "г.Санкт-Петербург,  Чудновского, № 8, корпус2, литер А",
                "г.Санкт-Петербург,  Швецова, № 4, литер А",
                "г.Санкт-Петербург,  Шевченко, № 38",
                "г.Санкт-Петербург,  Шелгунова, № 41, литер Д1",
                "г.Санкт-Петербург,  Шостаковича, № 1/9, литер А",
                "г.Санкт-Петербург,  Шотмана, № 11, литер Д",
                "г.Санкт-Петербург,  Шотмана, № 12, корпус1, литер Ж",
                "г.Санкт-Петербург,  Шотмана, № 5, корпус1, литер А",
                "г.Санкт-Петербург,  Шотмана, № 6, корпус1, литер З",
                "г.Санкт-Петербург,  Щербакова, № 27, корпус1, литер А",
                "г.Санкт-Петербург, 12-я линия В.О., № 19, литер А",
                "г.Санкт-Петербург, 12-я линия В.О., № 35",
                "г.Санкт-Петербург, 2-й Рабфаковский переулок, № 17, корпус1, литер А1",
                "г.Санкт-Петербург, 2-я Комсомольская , № 27, корпус1, литер А",
                "г.Санкт-Петербург, 2-я Комсомольская , № 7, корпус1, литер А",
                "г.Санкт-Петербург, 3-я Красноармейская , № 2/31, литер А",
                "г.Санкт-Петербург, 5-й Предпортовый проезд, № 1, литер А",
                "г.Санкт-Петербург, 5-я линия В.О., № 56",
                "г.Санкт-Петербург, 6-я Советская , № 18, литер А",
                "г.Санкт-Петербург, 8-я Советская , № 52",
                "г.Санкт-Петербург, Авиационная , № 9, литер А",
                "г.Санкт-Петербург, Автовская , № 15, корпус2, литер А",
                "г.Санкт-Петербург, Алтайская , № 18/19, литер А",
                "г.Санкт-Петербург, Альпийский переулок, № 30, литер А",
                "г.Санкт-Петербург, Альпийский переулок, № 5, корпус2, литер А",
                "г.Санкт-Петербург, Апрельская , № 5, литер А",
                "г.Санкт-Петербург, Афонская , № 14, корпус1",
                "г.Санкт-Петербург, Байконурская , № 15, литер А",
                "г.Санкт-Петербург, Байконурская , № 7, корпус1, литер А",
                "г.Санкт-Петербург, Байконурская , № 7, корпус2, литер А",
                "г.Санкт-Петербург, Балтийская , № 2/14, литер А",
                "г.Санкт-Петербург, Белорусская , № 16, корпус2",
                "г.Санкт-Петербург, Белорусская , № 28, литер А",
                "г.Санкт-Петербург, Бестужевская , № 49, литер А",
                "г.Санкт-Петербург, Бестужевская , № 79, литер А",
                "г.Санкт-Петербург, Благодатная , № 27",
                "г.Санкт-Петербург, Богатырский проспект, № 37",
                "г.Санкт-Петербург, Богатырский проспект, № 53, корпус1, литер А",
                "г.Санкт-Петербург, Богатырский проспект, № 7, корпус2, литер А",
                "г.Санкт-Петербург, Брестский бульвар, № 19/17, литер А",
                "г.Санкт-Петербург, Брюсовская , № 11, корпус2",
                "г.Санкт-Петербург, Брюсовская , № 5, корпус2, литер А",
                "г.Санкт-Петербург, Будапештская , № 14, корпус1, литер А",
                "г.Санкт-Петербург, Будапештская , № 14, корпус2",
                "г.Санкт-Петербург, Будапештская , № 17, корпус2",
                "г.Санкт-Петербург, Будапештская , № 48, литер А",
                "г.Санкт-Петербург, Бухарестская , № 13",
                "г.Санкт-Петербург, Бухарестская , № 41, корпус2, литер А",
                "г.Санкт-Петербург, Бухарестская , № 53",
                "г.Санкт-Петербург, Бухарестская , № 67, корпус1",
                "г.Санкт-Петербург, Бухарестская , № 67, корпус3",
                "г.Санкт-Петербург, Бухарестская , № 86, корпус2",
                "г.Санкт-Петербург, Варшавская , № 37, корпус1, литер А",
                "г.Санкт-Петербург, Варшавская , № 51, корпус1, литер А",
                "г.Санкт-Петербург, Витебский проспект, № 79, корпус3, литер А",
                "г.Санкт-Петербург, Володарский, Красносельское шоссе, № 54, литер А",
                "г.Санкт-Петербург, Выборгское шоссе, № 25, корпус1",
                "г.Санкт-Петербург, Выборгское шоссе, № 31",
                "г.Санкт-Петербург, Гаванская , № 32, литер А",
                "г.Санкт-Петербург, Гагаринская , № 3, литер А",
                "г.Санкт-Петербург, Галерная , № 54, литер А",
                "г.Санкт-Петербург, Гданьская , № 14, литер Б",
                "г.Санкт-Петербург, Горелово, Дачная , № 228",
                "г.Санкт-Петербург, Горелово, Красносельское шоссе, № 44, корпус3, литер А",
                "г.Санкт-Петербург, Горелово, Московская , № 87, литер А",
                "г.Санкт-Петербург, Горелово, Московская , № 95",
                "г.Санкт-Петербург, Гражданский проспект, № 106, корпус1",
                "г.Санкт-Петербург, Гражданский проспект, № 112, корпус1",
                "г.Санкт-Петербург, Гражданский проспект, № 114, корпус4",
                "г.Санкт-Петербург, Гражданский проспект, № 123, корпус1",
                "г.Санкт-Петербург, Гражданский проспект, № 19, корпус1, литер А",
                "г.Санкт-Петербург, Гражданский проспект, № 25, корпус2, литер А",
                "г.Санкт-Петербург, Гражданский проспект, № 27, корпус2, литер А",
                "г.Санкт-Петербург, Гражданский проспект, № 81",
                "г.Санкт-Петербург, Гражданский проспект, № 85, литер А",
                "г.Санкт-Петербург, Гражданский проспект, № 9, корпус8, литер А",
                "г.Санкт-Петербург, Гранитная , № 24",
                "г.Санкт-Петербург, Гранитная , № 54, корпус3, литер А",
                "г.Санкт-Петербург, Дальневосточный проспект, № 46, литер Д",
                "г.Санкт-Петербург, Дальневосточный проспект, № 64, литер Ф",
                "г.Санкт-Петербург, Дальневосточный проспект, № 72, литер Ш",
                "г.Санкт-Петербург, Дачный проспект, № 13, литер А",
                "г.Санкт-Петербург, Дачный проспект, № 16, корпус5",
                "г.Санкт-Петербург, Дачный проспект, № 23, корпус3, литер А",
                "г.Санкт-Петербург, Дачный проспект, № 24",
                "г.Санкт-Петербург, Двинская , № 10, литер А",
                "г.Санкт-Петербург, Дерптский переулок, № 9",
                "г.Санкт-Петербург, Детская , № 34",
                "г.Санкт-Петербург, Диагональная , № 4, корпус1",
                "г.Санкт-Петербург, Дибуновская , № 51",
                "г.Санкт-Петербург, Долгоозерная , № 6, корпус2",
                "г.Санкт-Петербург, Долгоозерная , № 9",
                "г.Санкт-Петербург, Дунайский проспект, № 26/77",
                "г.Санкт-Петербург, Дунайский проспект, № 28, корпус2, литер А",
                "г.Санкт-Петербург, Дунайский проспект, № 42/79, корпус1, литер А",
                "г.Санкт-Петербург, Загребский бульвар, № 23, корпус2, литер А",
                "г.Санкт-Петербург, Загребский бульвар, № 33, корпус2",
                "г.Санкт-Петербург, Загребский бульвар, № 35/28, литер А",
                "г.Санкт-Петербург, Замшина , № 68",
                "г.Санкт-Петербург, Заозерная , № 3, литер А",
                "г.Санкт-Петербург, Ивановская , № 36, литер А",
                "г.Санкт-Петербург, Индустриальный проспект, № 17, корпус3, литер А",
                "г.Санкт-Петербург, Индустриальный проспект, № 23, литер А",
                "г.Санкт-Петербург, Индустриальный проспект, № 24, литер А",
                "г.Санкт-Петербург, Индустриальный проспект, № 27, литер А",
                "г.Санкт-Петербург, Индустриальный проспект, № 29",
                "г.Санкт-Петербург, Индустриальный проспект, № 30/23, литер А",
                "г.Санкт-Петербург, Индустриальный проспект, № 38, корпус1, литер А",
                "г.Санкт-Петербург, Индустриальный проспект, № 38, корпус1, литер А",
                "г.Санкт-Петербург, Индустриальный проспект, № 9, литер А",
                "г.Санкт-Петербург, Искровский проспект, № 1/13, литер Е",
                "г.Санкт-Петербург, Искровский проспект, № 10, корпус2, литер К",
                "г.Санкт-Петербург, Искровский проспект, № 25, литер У",
                "г.Санкт-Петербург, Искровский проспект, № 27, литер Е",
                "г.Санкт-Петербург, Искровский проспект, № 28, литер Д",
                "г.Санкт-Петербург, Искровский проспект, № 6, корпус6, литер Ъ",
                "г.Санкт-Петербург, Искровский проспект, № 8, корпус3, литер Щ",
                "г.Санкт-Петербург, Исполкомская , № 5, литер А",
                "г.Санкт-Петербург, Камышовая , № 3",
                "г.Санкт-Петербург, Канонерский остров, № 12, литер А",
                "г.Санкт-Петербург, Канонерский остров, № 16, литер А",
                "г.Санкт-Петербург, Канонерский остров, № 22, литер А",
                "г.Санкт-Петербург, Канонерский остров, № 7",
                "г.Санкт-Петербург, Караваевская , № 4, литер А",
                "г.Санкт-Петербург, Кирочная , № 20, литер А",
                "г.Санкт-Петербург, Ключевая , № 3",
                "г.Санкт-Петербург, Коломяжский проспект, № 15, корпус2, литер А",
                "г.Санкт-Петербург, Коломяжский проспект, № 26, литер А",
                "г.Санкт-Петербург, Комендантский проспект, № 11, литер А",
                "г.Санкт-Петербург, Комендантский проспект, № 16, корпус2",
                "г.Санкт-Петербург, Комендантский проспект, № 21, корпус1, литер А",
                "г.Санкт-Петербург, Комендантский проспект, № 22, корпус3, литер А",
                "г.Санкт-Петербург, Комендантский проспект, № 37, корпус1, литер А",
                "г.Санкт-Петербург, Кондратьевский проспект, № 40, корпус6",
                "г.Санкт-Петербург, Кондратьевский проспект, № 65, литер А",
                "г.Санкт-Петербург, Косая линия, № 13",
                "г.Санкт-Петербург, Костромской проспект, № 37",
                "г.Санкт-Петербург, Краснопутиловская , № 101, литер А",
                "г.Санкт-Петербург, Краснопутиловская , № 43",
                "г.Санкт-Петербург, Краснопутиловская , № 54, литер А",
                "г.Санкт-Петербург, Краснопутиловская , № 83, литер А",
                "г.Санкт-Петербург, Краснопутиловская , № 84, литер А",
                "г.Санкт-Петербург, Крюкова , № 10",
                "г.Санкт-Петербург, Крюкова , № 11",
                "г.Санкт-Петербург, Кузнечный переулок, № 3, литер А",
                "г.Санкт-Петербург, Кузнечный переулок, № 9/27, литер А",
                "г.Санкт-Петербург, Купчинская , № 19, корпус1",
                "г.Санкт-Петербург, Купчинская , № 36",
                "г.Санкт-Петербург, Купчинская , № 4, корпус2, литер А",
                "г.Санкт-Петербург, Ланское шоссе, № 33, корпус1",
                "г.Санкт-Петербург, Ланское шоссе, № 65, литер А",
                "г.Санкт-Петербург, Лахта, Славянская , № 6, литер А",
                "г.Санкт-Петербург, Ленинский проспект, № 123, корпус2, литер А",
                "г.Санкт-Петербург, Ленинский проспект, № 129, литер А",
                "г.Санкт-Петербург, Ленинский проспект, № 134, литер А",
                "г.Санкт-Петербург, Ленинский проспект, № 135, литер А",
                "г.Санкт-Петербург, Ленинский проспект, № 147, литер А",
                "г.Санкт-Петербург, Ленинский проспект, № 154, литер А",
                "г.Санкт-Петербург, Ленинский проспект, № 157",
                "г.Санкт-Петербург, Ленинский проспект, № 95, корпус2, литер А",
                "г.Санкт-Петербург, Ленская , № 15, литер А",
                "г.Санкт-Петербург, Ленская , № 18, литер А",
                "г.Санкт-Петербург, Ленская , № 19, корпус1, литер А",
                "г.Санкт-Петербург, Ленская , № 21, литер А",
                "г.Санкт-Петербург, Ленская , № 6, корпус3, литер А",
                "г.Санкт-Петербург, Ленская , № 6, корпус4, литер А",
                "г.Санкт-Петербург, Лесной проспект, № 13/8, литер А",
                "г.Санкт-Петербург, Лесной проспект, № 39, корпус1",
                "г.Санкт-Петербург, Лиговский проспект, № 73, литер А",
                "г.Санкт-Петербург, Лиговский проспект, № 93, литер А",
                "г.Санкт-Петербург, Магнитогорская , № 95",
                "г.Санкт-Петербург, Малая Балканская , № 58, литер А",
                "г.Санкт-Петербург, Малый проспект П.С., № 16, литер А",
                "г.Санкт-Петербург, Мебельная , № 23, литер А",
                "г.Санкт-Петербург, Миргородская , № 3, литер А",
                "г.Санкт-Петербург, Московский проспект, № 138, литер А",
                "г.Санкт-Петербург, Московский проспект, № 182, литер А",
                "г.Санкт-Петербург, Московский проспект, № 206, литер А",
                "г.Санкт-Петербург, Московский проспект, № 207, сооружение 1, литер Г",
                "г.Санкт-Петербург, Московский проспект, № 208, литер А",
                "г.Санкт-Петербург, Моховая , № 44",
                "г.Санкт-Петербург, Наличная , № 44, корпус2, литер А",
                "г.Санкт-Петербург, Нарвский проспект, № 16, литер А",
                "г.Санкт-Петербург, Нарвский проспект, № 9, корпус2, литер А",
                "г.Санкт-Петербург, Народная , № 19, литер Ж",
                "г.Санкт-Петербург, Народная , № 41, литер В",
                "г.Санкт-Петербург, Народная , № 53, литер З",
                "г.Санкт-Петербург, Народная , № 59, литер К",
                "г.Санкт-Петербург, Народная , № 61, литер Л",
                "г.Санкт-Петербург, Народная , № 65, литер А",
                "г.Санкт-Петербург, Народная , № 67, литер М",
                "г.Санкт-Петербург, Народная , № 69, литер Н",
                "г.Санкт-Петербург, Народная , № 71, литер О",
                "г.Санкт-Петербург, Народная , № 90, литер З",
                "г.Санкт-Петербург, Невский проспект, № 132, литер А",
                "г.Санкт-Петербург, Невский проспект, № 166, литер А",
                "г.Санкт-Петербург, Невский проспект, № 38/4, литер А",
                "г.Санкт-Петербург, Новоизмайловский проспект, № 71, литер А",
                "г.Санкт-Петербург, Новосибирская , № 19",
                "г.Санкт-Петербург, Новочеркасский проспект, № 41/14, литер А",
                "г.Санкт-Петербург, Новочеркасский проспект, № 42",
                "г.Санкт-Петербург, Оборонная , № 7/20, литер А",
                "г.Санкт-Петербург, Огородный переулок, № 5, литер А",
                "г.Санкт-Петербург, Октябрьская набережная, № 100, корпус1, литер А",
                "г.Санкт-Петербург, Октябрьская набережная, № 100, корпус3",
                "г.Санкт-Петербург, Октябрьская набережная, № 122, корпус1",
                "г.Санкт-Петербург, Октябрьская набережная, № 90, корпус8, литер З1",
                "г.Санкт-Петербург, Ольховая , № 20",
                "г.Санкт-Петербург, Опочинина , № 15/18",
                "г.Санкт-Петербург, Парашютная , № 12, литер А",
                "г.Санкт-Петербург, Парашютная , № 4, корпус1, литер А",
                "г.Санкт-Петербург, Петергофское шоссе, № 17, корпус1, литер А",
                "г.Санкт-Петербург, Петергофское шоссе, № 19",
                "г.Санкт-Петербург, Планерная , № 49, литер А",
                "г.Санкт-Петербург, Пловдивская , № 3, корпус1, литер А",
                "г.Санкт-Петербург, Подольская , № 1-3-5, литер А",
                "г.Санкт-Петербург, Поцелуев мост, литер А",
                "г.Санкт-Петербург, Почтамтский переулок, № 2, литер А",
                "г.Санкт-Петербург, Пражская , № 15",
                "г.Санкт-Петербург, Пражская , № 37, корпус1",
                "г.Санкт-Петербург, Пражская , № 37, корпус2",
                "г.Санкт-Петербург, Пражская , № 7, корпус1, литер А",
                "г.Санкт-Петербург, Пражская , № 7, корпус2",
                "г.Санкт-Петербург, Придорожная аллея, № 23",
                "г.Санкт-Петербург, Придорожная аллея, № 31, литер А",
                "г.Санкт-Петербург, Придорожная аллея, № 5",
                "г.Санкт-Петербург, Придорожная аллея, № 9, корпус2, литер А",
                "г.Санкт-Петербург, Приморский проспект, № 37, литер А",
                "г.Санкт-Петербург, Псковская , № 20",
                "г.Санкт-Петербург, Пулковское шоссе, № 11, корпус1, литер А",
                "г.Санкт-Петербург, Пулковское шоссе, № 13, корпус2",
                "г.Санкт-Петербург, Пулковское шоссе, № 84, литер А",
                "г.Санкт-Петербург, Разъезжая , № 26-28, литер Б",
                "г.Санкт-Петербург, Рябиновая , № 2, литер А",
                "г.Санкт-Петербург, Светлановский проспект, № 107",
                "г.Санкт-Петербург, Светлановский проспект, № 115, корпус2",
                "г.Санкт-Петербург, Светлановский проспект, № 58, корпус2, литер А",
                "г.Санкт-Петербург, Светлановский проспект, № 63",
                "г.Санкт-Петербург, Светлановский проспект, № 66, литер А",
                "г.Санкт-Петербург, Светлановский проспект, № 72, корпус3, литер А",
                "г.Санкт-Петербург, Светлановский проспект, № 79, литер А",
                "г.Санкт-Петербург, Светлановский проспект, № 99, корпус1, литер А",
                "г.Санкт-Петербург, Северный проспект, № 16, корпус1",
                "г.Санкт-Петербург, Северный проспект, № 24, корпус1",
                "г.Санкт-Петербург, Северный проспект, № 61, корпус1, литер А",
                "г.Санкт-Петербург, Северный проспект, № 8, корпус2",
                "г.Санкт-Петербург, Сердобольская , № 11",
                "г.Санкт-Петербург, Сердобольская , № 31",
                "г.Санкт-Петербург, Серебристый бульвар, № 18, корпус2",
                "г.Санкт-Петербург, Серебристый бульвар, № 21, литер А",
                "г.Санкт-Петербург, Серебристый бульвар, № 22, корпус3, литер А",
                "г.Санкт-Петербург, Серебристый бульвар, № 26, литер А",
                "г.Санкт-Петербург, Серебристый бульвар, № 6, корпус1",
                "г.Санкт-Петербург, Ситцевая , № 11, корпус2",
                "г.Санкт-Петербург, Скачков переулок, № 3, литер А",
                "г.Санкт-Петербург, Софийская , № 27, литер А",
                "г.Санкт-Петербург, Софийская , № 31, литер А",
                "г.Санкт-Петербург, Софийская , № 35, корпус2",
                "г.Санкт-Петербург, Софийская , № 38, корпус1, литер А",
                "г.Санкт-Петербург, Софийская , № 60, литер А",
                "г.Санкт-Петербург, Среднеохтинский проспект, № 1, корпус2",
                "г.Санкт-Петербург, Стародеревенская , № 20, корпус2",
                "г.Санкт-Петербург, Суздальский проспект, № 1",
                "г.Санкт-Петербург, Суздальский проспект, № 105, корпус2",
                "г.Санкт-Петербург, Суздальский проспект, № 63, корпус3",
                "г.Санкт-Петербург, Суздальский проспект, № 67, корпус1",
                "г.Санкт-Петербург, Суздальский проспект, № 75, литер А",
                "г.Санкт-Петербург, Суздальский проспект, № 77, корпус1, литер А",
                "г.Санкт-Петербург, Суздальский проспект, № 77, корпус2, литер А",
                "г.Санкт-Петербург, Суздальский проспект, № 9, корпус1",
                "г.Санкт-Петербург, Суздальский проспект, № 91, корпус2",
                "г.Санкт-Петербург, Суздальский проспект, № 95, корпус1",
                "г.Санкт-Петербург, Тамбовская , № 75",
                "г.Санкт-Петербург, Тамбовская , № 78, литер А",
                "г.Санкт-Петербург, Татарский переулок, № 4, литер А",
                "г.Санкт-Петербург, Тимуровская , № 10, корпус3",
                "г.Санкт-Петербург, Тимуровская , № 11",
                "г.Санкт-Петербург, Тимуровская , № 23, корпус2",
                "г.Санкт-Петербург, Тимуровская , № 4, корпус1, литер А",
                "г.Санкт-Петербург, Тимуровская , № 7, корпус1",
                "г.Санкт-Петербург, Тихорецкий проспект, № 11, корпус3",
                "г.Санкт-Петербург, Товарищеский проспект, № 16 , корпус1, литер А",
                "г.Санкт-Петербург, Товарищеский проспект, № 28, корпус1, литер А",
                "г.Санкт-Петербург, Торфяная дорога, № 7, литер Т",
                "г.Санкт-Петербург, Трамвайный проспект, № 9, корпус2",
                "г.Санкт-Петербург, Тютчевская , № 7, литер А",
                "г.Санкт-Петербург, Учительская , № 8",
                "г.Санкт-Петербург, Фарфоровская , № 16, литер Н",
                "г.Санкт-Петербург, Хасанская , № 18, корпус2",
                "г.Санкт-Петербург, Хасанская , № 6, корпус1",
                "г.Санкт-Петербург, Хасанская , № 8, корпус1, литер А",
                "г.Санкт-Петербург, Шкиперский проток, № 12",
                "г.Санкт-Петербург, Школьная , № 76",
                "г.Санкт-Петербург, Шлиссельбургский проспект, № 1, литер А",
                "г.Санкт-Петербург, Шлиссельбургский проспект, № 39, корпус1, литер А",
                "г.Санкт-Петербург, Штурманская , № 42, корпус1",
                "г.Санкт-Петербург, Шувалово, Новоалександровская , № 21, литер А",
                "г.Санкт-Петербург, Шуваловский проспект, № 59, корпус1",
                "г.Санкт-Петербург, Южная дорога, № 5, литер А",
                "г.Санкт-Петербург, Якорная , № 6, литер А",
                "г.Санкт-Петербург, Яхтенная , № 22, корпус2",
                "г.Санкт-Петербург, Яхтенная , № 29",
                "г.Санкт-Петербург, аллея Котельникова, № 4, литер А",
                "г.Санкт-Петербург, бульвар Новаторов, № 116, литер А",
                "г.Санкт-Петербург, бульвар Новаторов, № 37",
                "г.Санкт-Петербург, бульвар Новаторов, № 40, литер А",
                "г.Санкт-Петербург, бульвар Новаторов, № 55",
                "г.Санкт-Петербург, бульвар Новаторов, № 59",
                "г.Санкт-Петербург, бульвар Новаторов, № 73, литер А",
                "г.Санкт-Петербург, набережная Крюкова канала, № 17, литер А",
                "г.Санкт-Петербург, набережная Обводного канала, № 157",
                "г.Санкт-Петербург, набережная Обводного канала, № 74, литер А",
                "г.Санкт-Петербург, набережная Обводного канала, № 96, литер А",
                "г.Санкт-Петербург, набережная канала Грибоедова, № 76, литер П",
                "г.Санкт-Петербург, набережная реки Карповки, № 22, корпус2",
                "г.Санкт-Петербург, набережная реки Карповки, № 45, литер А",
                "г.Санкт-Петербург, набережная реки Фонтанки, № 131, литер А",
                "г.Санкт-Петербург, переулок Пирогова, № 5",
                "г.Санкт-Петербург, площадь Карла Фаберже, № 8, литер А",
                "г.Санкт-Петербург, площадь Ленина, № 3, сооружение 3, литер А",
                "г.Санкт-Петербург, площадь Победы, № 2, литер А",
                "г.Санкт-Петербург, проспект Авиаконструкторов, № 17, корпус2, литер А",
                "г.Санкт-Петербург, проспект Авиаконструкторов, № 20, корпус1, литер А",
                "г.Санкт-Петербург, проспект Авиаконструкторов, № 32, литер А",
                "г.Санкт-Петербург, проспект Авиаконструкторов, № 38, корпус1, литер А",
                "г.Санкт-Петербург, проспект Авиаконструкторов, № 42, корпус2, литер А",
                "г.Санкт-Петербург, проспект Александровской Фермы, № 7, литер В",
                "г.Санкт-Петербург, проспект Большевиков, № 11/19, литер А",
                "г.Санкт-Петербург, проспект Большевиков, № 13, корпус2, литер В",
                "г.Санкт-Петербург, проспект Большевиков, № 13, корпус3, литер М",
                "г.Санкт-Петербург, проспект Большевиков, № 17, литер Ч",
                "г.Санкт-Петербург, проспект Большевиков, № 19, литер Ш",
                "г.Санкт-Петербург, проспект Большевиков, № 2, литер А",
                "г.Санкт-Петербург, проспект Большевиков, № 37, корпус1, литер А",
                "г.Санкт-Петербург, проспект Большевиков, № 4, корпус1, литер Ш",
                "г.Санкт-Петербург, проспект Большевиков, № 57, корпус3, литер А",
                "г.Санкт-Петербург, проспект Большевиков, № 63, корпус4, литер К1",
                "г.Санкт-Петербург, проспект Большевиков, № 67, корпус3, литер Б1",
                "г.Санкт-Петербург, проспект Большевиков, № 67, корпус4, литер Г",
                "г.Санкт-Петербург, проспект Большевиков, № 75, корпус2, литер Г",
                "г.Санкт-Петербург, проспект Большевиков, № 8, корпус1, литер А",
                "г.Санкт-Петербург, проспект Ветеранов, № 112, литер А",
                "г.Санкт-Петербург, проспект Ветеранов, № 142, литер А",
                "г.Санкт-Петербург, проспект Ветеранов, № 17",
                "г.Санкт-Петербург, проспект Ветеранов, № 27",
                "г.Санкт-Петербург, проспект Ветеранов, № 31",
                "г.Санкт-Петербург, проспект Ветеранов, № 43, литер А",
                "г.Санкт-Петербург, проспект Ветеранов, № 49",
                "г.Санкт-Петербург, проспект Ветеранов, № 99, литер А",
                "г.Санкт-Петербург, проспект Елизарова, № 17, литер А",
                "г.Санкт-Петербург, проспект Елизарова, № 35, литер А",
                "г.Санкт-Петербург, проспект Елизарова, № 37, литер А",
                "г.Санкт-Петербург, проспект Испытателей, № 2, корпус6, литер Б",
                "г.Санкт-Петербург, проспект Королева, № 26, корпус1",
                "г.Санкт-Петербург, проспект Королева, № 26, корпус1",
                "г.Санкт-Петербург, проспект Королева, № 32, корпус2",
                "г.Санкт-Петербург, проспект Королева, № 42, корпус1",
                "г.Санкт-Петербург, проспект Королева, № 44, корпус1",
                "г.Санкт-Петербург, проспект Королева, № 48, корпус4",
                "г.Санкт-Петербург, проспект Королева, № 49, литер А",
                "г.Санкт-Петербург, проспект Королева, № 9, литер А",
                "г.Санкт-Петербург, проспект Королева, № 9, литер А",
                "г.Санкт-Петербург, проспект Косыгина, № 15, литер А",
                "г.Санкт-Петербург, проспект Косыгина, № 23, корпус1, литер А",
                "г.Санкт-Петербург, проспект Косыгина, № 26, корпус1, литер А",
                "г.Санкт-Петербург, проспект Косыгина, № 27, корпус1, литер А",
                "г.Санкт-Петербург, проспект Косыгина, № 28, корпус4",
                "г.Санкт-Петербург, проспект Косыгина, № 30, корпус2, литер А",
                "г.Санкт-Петербург, проспект Косыгина, № 34, корпус1, литер А",
                "г.Санкт-Петербург, проспект Косыгина, № 9, корпус2",
                "г.Санкт-Петербург, проспект Кузнецова, № 29, корпус1",
                "г.Санкт-Петербург, проспект Кузнецова, № 32, литер А",
                "г.Санкт-Петербург, проспект Культуры, № 11, корпус7",
                "г.Санкт-Петербург, проспект Культуры, № 15, корпус2",
                "г.Санкт-Петербург, проспект Культуры, № 27, корпус3, литер А",
                "г.Санкт-Петербург, проспект Луначарского, № 21, корпус3",
                "г.Санкт-Петербург, проспект Луначарского, № 21, корпус4",
                "г.Санкт-Петербург, проспект Луначарского, № 33, корпус2",
                "г.Санкт-Петербург, проспект Луначарского, № 38",
                "г.Санкт-Петербург, проспект Луначарского, № 39, корпус1",
                "г.Санкт-Петербург, проспект Луначарского, № 58, корпус2",
                "г.Санкт-Петербург, проспект Луначарского, № 66, корпус3, литер А",
                "г.Санкт-Петербург, проспект Луначарского, № 68, корпус2, литер А",
                "г.Санкт-Петербург, проспект Луначарского, № 70, корпус2",
                "г.Санкт-Петербург, проспект Луначарского, № 78, корпус1",
                "г.Санкт-Петербург, проспект Луначарского, № 80, корпус4",
                "г.Санкт-Петербург, проспект Маршала Блюхера, № 21, корпус1",
                "г.Санкт-Петербург, проспект Маршала Блюхера, № 61, корпус1, литер А",
                "г.Санкт-Петербург, проспект Маршала Блюхера, № 61, корпус2",
                "г.Санкт-Петербург, проспект Маршала Жукова, № 22, литер А",
                "г.Санкт-Петербург, проспект Маршала Жукова, № 26/16, литер В",
                "г.Санкт-Петербург, проспект Маршала Жукова, № 28, корпус2, литер Б",
                "г.Санкт-Петербург, проспект Маршала Жукова, № 28, корпус3, литер А",
                "г.Санкт-Петербург, проспект Маршала Жукова, № 34",
                "г.Санкт-Петербург, проспект Маршала Жукова, № 60, корпус1, литер Г",
                "г.Санкт-Петербург, проспект Маршала Жукова, № 60, корпус2, литер А",
                "г.Санкт-Петербург, проспект Маршала Жукова, № 72, корпус2, литер А",
                "г.Санкт-Петербург, проспект Металлистов, № 102",
                "г.Санкт-Петербург, проспект Металлистов, № 80, корпус1, литер А",
                "г.Санкт-Петербург, проспект Мечникова, № 10, литер А",
                "г.Санкт-Петербург, проспект Мечникова, № 3, корпус1",
                "г.Санкт-Петербург, проспект Мечникова, № 3, литер А",
                "г.Санкт-Петербург, проспект Народного Ополчения, № 113",
                "г.Санкт-Петербург, проспект Народного Ополчения, № 159",
                "г.Санкт-Петербург, проспект Народного Ополчения, № 161",
                "г.Санкт-Петербург, проспект Народного Ополчения, № 183, литер А",
                "г.Санкт-Петербург, проспект Народного Ополчения, № 205",
                "г.Санкт-Петербург, проспект Народного Ополчения, № 221, литер А",
                "г.Санкт-Петербург, проспект Народного Ополчения, № 24, литер А",
                "г.Санкт-Петербург, проспект Народного Ополчения, № 73",
                "г.Санкт-Петербург, проспект Народного Ополчения, № 83",
                "г.Санкт-Петербург, проспект Наставников, № 10",
                "г.Санкт-Петербург, проспект Наставников, № 24, корпус1, литер А",
                "г.Санкт-Петербург, проспект Наставников, № 28, корпус2",
                "г.Санкт-Петербург, проспект Науки, № 10, корпус2, литер А",
                "г.Санкт-Петербург, проспект Науки, № 27",
                "г.Санкт-Петербург, проспект Науки, № 37",
                "г.Санкт-Петербург, проспект Науки, № 53",
                "г.Санкт-Петербург, проспект Науки, № 73, корпус1",
                "г.Санкт-Петербург, проспект Обуховской Обороны, № 269, корпус2, литер А",
                "г.Санкт-Петербург, проспект Пархоменко, № 27, корпус2",
                "г.Санкт-Петербург, проспект Пархоменко, № 45, корпус1, литер А",
                "г.Санкт-Петербург, проспект Просвещения, № 20/25",
                "г.Санкт-Петербург, проспект Просвещения, № 24/2, литер А",
                "г.Санкт-Петербург, проспект Просвещения, № 32, корпус2",
                "г.Санкт-Петербург, проспект Просвещения, № 46, корпус2",
                "г.Санкт-Петербург, проспект Просвещения, № 48, литер А",
                "г.Санкт-Петербург, проспект Пятилеток, № 5/12, литер А",
                "г.Санкт-Петербург, проспект Раевского, № 5, литер Н",
                "г.Санкт-Петербург, проспект Сизова, № 21, литер А",
                "г.Санкт-Петербург, проспект Славы, № 12, корпус3, литер А",
                "г.Санкт-Петербург, проспект Славы, № 23, корпус4, литер А",
                "г.Санкт-Петербург, проспект Славы, № 41",
                "г.Санкт-Петербург, проспект Славы, № 43/49, литер А",
                "г.Санкт-Петербург, проспект Славы, № 43/49, литер А",
                "г.Санкт-Петербург, проспект Солидарности, № 1, корпус1, литер А",
                "г.Санкт-Петербург, проспект Солидарности, № 1, корпус3, литер А",
                "г.Санкт-Петербург, проспект Солидарности, № 12, корпус1, литер А",
                "г.Санкт-Петербург, проспект Солидарности, № 19, литер Д",
                "г.Санкт-Петербург, проспект Солидарности, № 3, корпус2, литер А",
                "г.Санкт-Петербург, проспект Солидарности, № 5, литер А",
                "г.Санкт-Петербург, проспект Солидарности, № 7, корпус1, литер И",
                "г.Санкт-Петербург, проспект Солидарности, № 7, корпус4, литер Х",
                "г.Санкт-Петербург, проспект Солидарности, № 8, корпус4, литер Е",
                "г.Санкт-Петербург, проспект Солидарности, № 9, корпус2, литер П",
                "г.Санкт-Петербург, проспект Стачек, № 101, корпус1, литер А",
                "г.Санкт-Петербург, проспект Стачек, № 103, корпус1, литер А",
                "г.Санкт-Петербург, проспект Стачек, № 105, корпус2, литер Е",
                "г.Санкт-Петербург, проспект Стачек, № 107, корпус1, литер А",
                "г.Санкт-Петербург, проспект Стачек, № 21, литер А",
                "г.Санкт-Петербург, проспект Стачек, № 24, литер Щ",
                "г.Санкт-Петербург, проспект Стачек, № 55, литер А",
                "г.Санкт-Петербург, проспект Стачек, № 69, литер А",
                "г.Санкт-Петербург, проспект Стачек, № 70, литер А",
                "г.Санкт-Петербург, проспект Стачек, № 74/1, литер А",
                "г.Санкт-Петербург, проспект Стачек, № 80, литер А",
                "г.Санкт-Петербург, проспект Тореза, № 102, корпус5",
                "г.Санкт-Петербург, проспект Тореза, № 25",*/
                "г.Санкт-Петербург, проспект Тореза, № 77, корпус1",
                "г.Санкт-Петербург, проспект Ударников, № 15, корпус1",
                "г.Санкт-Петербург, проспект Ударников, № 21, корпус2",
                "г.Санкт-Петербург, проспект Ударников, № 32, корпус3, литер А",
                "г.Санкт-Петербург, проспект Ударников, № 43, корпус1, литер А",
                "г.Санкт-Петербург, проспект Художников, № 18, корпус1",
                "г.Санкт-Петербург, проспект Художников, № 20, корпус1",
                "г.Санкт-Петербург, проспект Художников, № 31, корпус1",
                "г.Санкт-Петербург, проспект Шаумяна, № 77",
                "г.Санкт-Петербург, проспект Энгельса, № 121, корпус1, литер А",
                "г.Санкт-Петербург, проспект Энгельса, № 125, литер А",
                "г.Санкт-Петербург, проспект Энгельса, № 138, корпус1, литер А",
                "г.Санкт-Петербург, проспект Энгельса, № 143, корпус3",
                "г.Санкт-Петербург, проспект Энгельса, № 145, корпус3",
                "г.Санкт-Петербург, проспект Энгельса, № 147, корпус2",
                "г.Санкт-Петербург, проспект Энгельса, № 151, корпус1",
                "г.Санкт-Петербург, проспект Энергетиков, № 24, литер А",
                "г.Санкт-Петербург, проспект Энергетиков, № 28, корпус5, литер А",
                "г.Санкт-Петербург, проспект Энергетиков, № 72",
                "г.Санкт-Петербург, проспект Энтузиастов, № 22, корпус1",
                "г.Санкт-Петербург, проспект Энтузиастов, № 40, корпус2",
                "г.Санкт-Петербург, проспект Энтузиастов, № 47, корпус1",
                "г.Санкт-Петербург, проспект Энтузиастов, № 51, корпус2, литер А",
                "г.Санкт-Петербург, проспект Юрия Гагарина, № 11",
                "г.Санкт-Петербург, проспект Юрия Гагарина, № 59",
                "г.Санкт-Петербург, шоссе Революции, № 21, корпус2",
            };

            var keys = new Queue<string>();


            while (locations.Count() > 20)
                locations.RemoveAt(0);
            int ctn = 0;
            foreach (var location in locations)
            {
                Logger.LogInformation(location);

                if ((ctn++) == 10)
                    break;
                var holder = new ProductHolder<TItem>()
                {
                    HolderIsActive = false,
                    HolderLocation = location,
                    HolderSerial = keys.FirstOrDefault()
                };
                string sk = "";
                keys.TryDequeue(out sk);
                _deliveryDbContext.Holders.Add(Resolve(holder));
                ctn += _deliveryDbContext.SaveChanges();



                foreach (var product in _deliveryDbContext.Products.ToList())
                {
                    var pis = new ProductsInStock()
                    {

                        HolderID = holder.ID,
                        ProductID = product.ID,
                        ProductCount = GetRandomNumber(100)
                    };
                    Logger.LogInformation(SerializeObject(pis));
                    _deliveryDbContext.ProductsInStock.Add(pis);
                }
                ctn += _deliveryDbContext.SaveChanges();
                Logger.LogInformation(SerializeObject(holder));

                if (_deliveryDbContext.Holders.Count() > 100)
                    break;


            }
         
            return ctn;
        }

        public THolder Resolve(ProductHolder<TItem> holder) => (THolder)typeof(THolder).GetConstructors().FirstOrDefault().Invoke(new object[0]).CopyData(holder);

        private int GetRandomNumber(int max)
        {
            return new Random().Next(max);
        }

        private T GetRandom<T>(IEnumerable<T> items)
        {
            return items.ToArray()[GetRandomNumber(items.Count())];
        }


        public virtual int InitProducts(DeliveryDbContext<TCustomer, TItem, THolder> context, string contentPath)
        {
            string ResourceDir = $"{contentPath}" + @"\PhoneMarketplace";
            if (System.IO.Directory.Exists(ResourceDir) == false)
                throw new Exception("Не удалось загрузить сведения о продукции, т.к. не корректно указан путь к директории: " + ResourceDir);
            int ctn = 0;
            if (context != null && context.Products != null && context.Products.Count() > 0)
                return 0;
            foreach (var file in System.IO.Directory.GetFiles(ResourceDir + @"\phones"))
            {
                //TODO: вычленить имя файла для определния модели телефона
                //WriteLine($"file: {file}");
                string phoneModel = file.Substring(file.LastIndexOf(@"\") + 1);
                string fileExt = phoneModel.Substring(phoneModel.LastIndexOf(".") + 1);
                phoneModel = phoneModel.Substring(0, phoneModel.LastIndexOf("."));

                string productOptionsJson = System.IO.File.ReadAllText(file);
                WriteLine(productOptionsJson);


                ProductItem production = null;
                context.Products.Add(production = new ProductItem()
                {
                    ProductName = phoneModel,
                    ProductIndicatorsJson = productOptionsJson
                });
                context.SaveChanges();
                ctn++;

                foreach (var imgPath in System.IO.Directory.GetFiles(ResourceDir + @"\phones-img").Where(path => path.Contains(phoneModel)))
                {
                    string contentType = file.Substring(file.LastIndexOf(".") + 1);
                    byte[] imgData = System.IO.File.ReadAllBytes(imgPath);
                    var phoneImage = new ProductImage()
                    {
                        ContentType = $"image/{contentType}",
                        ImageData = imgData,
                        ProductID = production.ID,
                    };
                    context.ProductImages.Add(phoneImage);
                    context.SaveChanges();
                }
                WriteLine(SerializeObject(production));
            }
            return ctn;
        }

    } 
    public static class StringNormalizationExtensions
    {

        public static string ReplaceAll(this string text, string s1, string s2)
        {
            while (text.IndexOf(s1) != -1)
            {
                text = text.Replace(s1, s2);
            }
            return text;
        }
    }
}
