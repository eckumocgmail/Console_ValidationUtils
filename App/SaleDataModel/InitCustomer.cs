using DataEntities;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcDataApp.DataEntities.DataInit
{
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
}
