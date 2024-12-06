using System.Net.Mime;
using System.Text;
using System.Xml;
using Apps.Memoq.Contracts;
using Apps.Memoq.Models;
using Apps.Memoq.Models.Termbases;
using Apps.Memoq.Models.Termbases.Requests;
using Apps.Memoq.Models.Termbases.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Glossaries.Utils.Converters;
using Blackbird.Applications.Sdk.Glossaries.Utils.Dtos;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using MQS.TB;
using Apps.MemoQ.Models.Termbases.Requests;

namespace Apps.Memoq.Actions;

[ActionList]
public class TermBaseActions : BaseInvocable
{
    private readonly IFileManagementClient _fileManagementClient;
    
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public TermBaseActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) 
        : base(invocationContext)
    {
        _fileManagementClient = fileManagementClient;
    }

    #region Import
    
    private const string EntryId = "Entry_ID";
    private const string EntrySubject = "Entry_Subject";
    private const string EntryDomain = "Entry_Domain";
    private const string EntryClientId = "Entry_ClientID";
    private const string EntryProjectId = "Entry_ProjectID";
    private const string EntryCreated = "Entry_Created";
    private const string EntryCreator = "Entry_Creator";
    private const string EntryLastModified = "Entry_LastModified";
    private const string EntryModifier = "Entry_Modifier";
    private const string EntryNote = "Entry_Note"; 
    private const string Definition = "Def"; 
    private const string TermInfo = "Term_Info"; 
    private const string TermExample = "Term_Example";

    private readonly Dictionary<string, string> _csvFileLanguages = new()
    {
        ["afr"] = "Afrikaans", ["aka"] = "Akan", ["alb"] = "Albanian", ["alb-AL"] = "Albanian_Albania",
        ["alb-XK"] = "Albanian_Kosovo", ["alb-MK"] = "Albanian_Macedonia", ["alb-ME"] = "Albanian_Montenegro",
        ["amh"] = "Amharic", ["ara"] = "Arabic", ["ara-DZ"] = "Arabic_Algeria", ["ara-BH"] = "Arabic_Bahrain",
        ["ara-EG"] = "Arabic_Egypt", ["ara-IQ"] = "Arabic_Iraq", ["ara-JO"] = "Arabic_Jordan",
        ["ara-KW"] = "Arabic_Kuwait", ["ara-LB"] = "Arabic_Lebanon", ["ara-LY"] = "Arabic_Libya",
        ["ara-MA"] = "Arabic_Morocco", ["ara-OM"] = "Arabic_Oman", ["ara-QA"] = "Arabic_Qatar",
        ["ara-SA"] = "Arabic_Saudi_Arabia", ["ara-SY"] = "Arabic_Syria", ["ara-TN"] = "Arabic_Tunisia",
        ["ara-AE"] = "Arabic_U.A.E.", ["ara-YE"] = "Arabic_Yemen", ["arg"] = "Aragonese", ["ocs"] = "Aranese",
        ["hye"] = "Armenian", ["asm"] = "Assamese", ["ast"] = "Asturian", ["azf"] = "Azeri_Cyrillic",
        ["aze"] = "Azeri_Latin", ["bxg"] = "Bangala", ["baq"] = "Basque", ["bel"] = "Belarussian",
        ["ben"] = "Bengali", ["ben-BD"] = "Bengali_Bangladesh", ["ben-IN"] = "Bengali_India", ["bis"] = "Bislama",
        ["boc"] = "Bosnian_Cyrillic", ["bos"] = "Bosnian_Latin", ["bre"] = "Breton", ["bul"] = "Bulgarian",
        ["mya"] = "Burmese", ["cat"] = "Catalan", ["ceb"] = "Cebuano", ["chr"] = "Cherokee", ["ctd"] = "Chin",
        ["zho-HK"] = "Chinese_Hong_Kong_S.A.R.", ["zho-MO"] = "Chinese_Macao_S.A.R.", ["zho-CN"] = "Chinese_PRC",
        ["zho-SG"] = "Chinese_Singapore", ["zho-TW"] = "Chinese_Taiwan", ["cho"] = "Choctaw", ["chk"] = "Chuukese",
        ["hrv"] = "Croatian", ["cze"] = "Czech", ["dan"] = "Danish", ["prs"] = "Dari", ["din"] = "Dinka",
        ["dut"] = "Dutch", ["dut-BE"] = "Dutch_Belgium", ["dut-NL"] = "Dutch_Netherlands", ["eng"] = "English",
        ["eng-AU"] = "English_Australia", ["eng-BZ"] = "English_Belize", ["eng-CA"] = "English_Canada",
        ["eng-CB"] = "English_Caribbean", ["eng-IE"] = "English_Ireland", ["eng-JM"] = "English_Jamaica",
        ["eng-NZ"] = "English_New_Zealand", ["eng-PH"] = "English_Republic_of_the_Philippines",
        ["eng-ZA"] = "English_South_Africa", ["eng-TT"] = "English_Trinidad_and_Tobago",
        ["eng-GB"] = "English_United_Kingdom", ["eng-US"] = "English_United_States",
        ["eng-ZW"] = "English_Zimbabwe", ["epo"] = "Esperanto", ["est"] = "Estonian", ["fat"] = "Fanti",
        ["fas"] = "Farsi", ["fij"] = "Fijian", ["fil"] = "Filipino", ["fin"] = "Finnish", ["vls"] = "Flemish",
        ["fre"] = "French", ["fre-02"] = "French_Africa", ["fre-BE"] = "French_Belgium",
        ["fre-CA"] = "French_Canada", ["fre-FR"] = "French_France", ["fre-LU"] = "French_Luxembourg",
        ["fre-MC"] = "French_Monaco", ["fre-MA"] = "French_Morocco", ["fre-CH"] = "French_Switzerland",
        ["fry"] = "Frisian,_Western", ["ful"] = "Fulah", ["gla"] = "Gaelic_Scotland", ["glg"] = "Galician",
        ["kat"] = "Georgian", ["ger"] = "German", ["ger-AT"] = "German_Austria", ["ger-DE"] = "German_Germany",
        ["ger-LI"] = "German_Liechtenstein", ["ger-LU"] = "German_Luxembourg", ["ger-CH"] = "German_Switzerland",
        ["gre"] = "Greek", ["kal"] = "Greenlandic", ["grn"] = "Guaraní", ["guj"] = "Gujarati",
        ["hat"] = "Haitian_Creole", ["hau"] = "Hausa", ["haw"] = "Hawaiian", ["haz"] = "Hazaragi", ["heb"] = "Hebrew",
        ["hil"] = "Hiligaynon", ["hin"] = "Hindi", ["hmn"] = "Hmong", ["hun"] = "Hungarian", ["ice"] = "Icelandic",
        ["ibo"] = "Igbo", ["ilo"] = "Ilocano", ["ind"] = "Indonesian", ["gle"] = "Irish", ["ita"] = "Italian",
        ["ita-IT"] = "Italian_Italy", ["ita-CH"] = "Italian_Switzerland", ["jpn"] = "Japanese",
        ["jav"] = "Javanese", ["kea"] = "Kabuverdianu", ["kan"] = "Kannada", ["ksw"] = "Karen", ["kas"] = "Kashmiri",
        ["kyu"] = "Kayah_Latin", ["eky"] = "Kayah_Myanmar", ["kaz"] = "Kazakh", ["khm"] = "Khmer",
        ["gil"] = "Kiribati", ["qqq"] = "Klingon", ["kor"] = "Korean", ["ckb"] = "Kurdish_Arabic", 
        ["kmr"] = "Kurdish_Cyrillic", ["kur"] = "Kurdish_Latin", ["kir"] = "Kyrgyz_Cyrillic", ["lao"] = "Lao",
        ["lat"] = "Latin", ["lav"] = "Latvian", ["lin"] = "Lingala", ["lit"] = "Lithuanian", ["ltz"] = "Luxembourgish",
        ["ymm"] = "Maay", ["mac"] = "Macedonian", ["mlg"] = "Malagasy", ["msa"] = "Malay", ["mal"] = "Malayalam",
        ["mlt"] = "Maltese", ["mno"] = "Mandinka_Arabic", ["mnk"] = "Mandinka_Latin", ["mri"] = "Maori",
        ["mar"] = "Marathi", ["mah"] = "Marshallese", ["fit"] = "Meänkieli", ["mol"] = "Moldavian", ["mnw"] = "Mon",
        ["khk"] = "Mongolian_Cyrillic", ["cgy"] = "Montenegrin_Cyrillic", ["cgl"] = "Montenegrin_Latin",
        ["nau"] = "Nauruan", ["nav"] = "Navajo", ["nep"] = "Nepali", ["nor"] = "Norwegian",
        ["nnb"] = "Norwegian_Bokmål", ["nno"] = "Norwegian_Nynorsk", ["oci"] = "Occitan", ["ori"] = "Oriya",
        ["orm"] = "Oromo", ["pbu"] = "Pashto", ["pdc"] = "Pennsylvania_German", ["pis"] = "Pijin",
        ["pon"] = "Pohnpeian", ["pol"] = "Polish", ["por"] = "Portuguese", ["por-BR"] = "Portuguese_Brazil",
        ["por-PT"] = "Portuguese_Portugal", ["pan"] = "Punjabi_Gurmukhi", ["pnb"] = "Punjabi_Shahmukhi",
        ["quz"] = "Quechua", ["rki"] = "Rakhine", ["rhg"] = "Rohingya", ["rum"] = "Romanian", ["run"] = "Rundi",
        ["rus"] = "Russian", ["kin"] = "Rwanda", ["smo"] = "Samoan", ["san"] = "Sanskrit",
        ["scc"] = "Serbian_Cyrillic", ["scr"] = "Serbian_Latin", ["sot"] = "Sesotho", ["shn"] = "Shan",
        ["sin"] = "Sinhala", ["slo"] = "Slovak", ["slv"] = "Slovenian", ["som"] = "Somali",
        ["som-DJ"] = "Somali_Djibouti", ["som-ET"] = "Somali_Ethiopia", ["som-KE"] = "Somali_Kenya",
        ["som-SO"] = "Somali_Somalia", ["spa"] = "Spanish", ["spa-AR"] = "Spanish_Argentina",
        ["spa-BO"] = "Spanish_Bolivia", ["spa-CL"] = "Spanish_Chile", ["spa-CO"] = "Spanish_Colombia",
        ["spa-CR"] = "Spanish_Costa_Rica", ["spa-DO"] = "Spanish_Dominican_Republic",
        ["spa-EC"] = "Spanish_Ecuador", ["spa-SV"] = "Spanish_El_Salvador", ["spa-GT"] = "Spanish_Guatemala",
        ["spa-HN"] = "Spanish_Honduras", ["spa-M9"] = "Spanish_Latin_America", ["spa-MX"] = "Spanish_Mexico",
        ["spa-NI"] = "Spanish_Nicaragua", ["spa-PA"] = "Spanish_Panama", ["spa-PY"] = "Spanish_Paraguay",
        ["spa-PE"] = "Spanish_Peru", ["spa-PR"] = "Spanish_Puerto_Rico", ["spa-ES"] = "Spanish_Spain",
        ["spa-US"] = "Spanish_United_States", ["spa-UY"] = "Spanish_Uruguay", ["spa-VE"] = "Spanish_Venezuela",
        ["pga"] = "Sudanese_Creole_Arabic", ["sun"] = "Sundanese", ["swa"] = "Swahili", ["swe"] = "Swedish",
        ["swe-FI"] = "Swedish_Finland", ["swe-SE"] = "Swedish_Sweden", ["tgl"] = "Tagalog",
        ["tgk"] = "Tajiki_Cyrillic", ["tzm"] = "Tamazight", ["tam"] = "Tamil", ["tat"] = "Tatar", ["tel"] = "Telugu",
        ["tdt"] = "Tetun_Dili", ["tha"] = "Thai", ["tir"] = "Tigrigna", ["tpi"] = "Tok_Pisin", ["ton"] = "Tongan",
        ["tcs"] = "Torres_Strait_Creole", ["tsn"] = "Tswana", ["tur"] = "Turkish", ["tuk"] = "Turkmen_Latin",
        ["tvl"] = "Tuvaluan", ["twi"] = "Twi", ["ukr"] = "Ukrainian", ["urd"] = "Urdu", ["uzn"] = "Uzbek_Cyrillic",
        ["uzb"] = "Uzbek_Latin", ["vie"] = "Vietnamese", ["wel"] = "Welsh", ["wol"] = "Wolof", ["xho"] = "Xhosa",
        ["yid"] = "Yiddish", ["yor"] = "Yoruba", ["zul"] = "Zulu"
    };
    
    private readonly Dictionary<string, string> _tbxMemoQLanguages = new()
    {
        ["af"] = "afr", ["ak"] = "aka", ["sq"] = "alb", ["sq-al"] = "alb-AL", ["sq-xk"] = "alb-XK", 
        ["sq-mk"] = "alb-MK", ["sq-me"] = "alb-ME", ["am"] = "amh", ["ar"] = "ara", ["ar-dz"] = "ara-DZ", 
        ["ar-bh"] = "ara-BH", ["ar-eg"] = "ara-EG", ["ar-iq"] = "ara-IQ", ["ar-jo"] = "ara-JO", ["ar-kw"] = "ara-KW", 
        ["ar-lb"] = "ara-LB", ["ar-ly"] = "ara-LY", ["ar-ma"] = "ara-MA", ["ar-om"] = "ara-OM", ["ar-qa"] = "ara-QA", 
        ["ar-sa"] = "ara-SA", ["ar-sy"] = "ara-SY", ["ar-tn"] = "ara-TN", ["ar-ae"] = "ara-AE", ["ar-ye"] = "ara-YE", 
        ["an"] = "arg", ["oc"] = "ocs", ["hy"] = "hye", ["as"] = "asm", ["ast"] = "ast", ["azf"] = "azf", 
        ["az-cyrl-az"] = "azf", ["az-cyrl"] = "azf", ["aze"] = "aze", ["az-latn-az"] = "aze", ["az-latn"] = "aze", 
        ["bx"] = "bxg", ["eu"] = "baq", ["be"] = "bel", ["bn"] = "ben", ["bn-bd"] = "ben-BD", ["bn-in"] = "ben-IN",
        ["boc"] = "boc", ["bs-cyrl-ba"] = "boc", ["bs-cyrl"] = "boc", ["bos"] = "bos", ["bs-latn-ba"] = "bos", 
        ["bs-latn"] = "bos", ["bi"] = "bis", ["br"] = "bre", ["bg"] = "bul", ["my"] = "mya", ["ca"] = "cat", 
        ["ceb"] = "ceb", ["chr"] = "chr", ["ctd"] = "ctd", ["zh-hk"] = "zho-HK", ["zh-mo"] = "zho-MO", 
        ["zh-cn"] = "zho-CN", ["zh-sg"] = "zho-SG", ["zh-tw"] = "zho-TW", ["cho"] = "cho", ["chk"] = "chk", 
        ["hr"] = "hrv", ["cs"] = "cze", ["da"] = "dan", ["prs"] = "prs", ["din"] = "din", ["nl"] = "dut", 
        ["nl-be"] = "dut-BE", ["nl-nl"] = "dut-NL", ["en"] = "eng", ["en-au"] = "eng-AU", ["en-bz"] = "eng-BZ", 
        ["en-ca"] = "eng-CA", ["en-cb"] = "eng-CB", ["en-ie"] = "eng-IE", ["en-jm"] = "eng-JM", ["en-nz"] = "eng-NZ", 
        ["en-ph"] = "eng-PH", ["en-za"] = "eng-ZA", ["en-tt"] = "eng-TT", ["en-gb"] = "eng-GB", ["en-us"] = "eng-US", 
        ["en-zw"] = "eng-ZW", ["eo"] = "epo", ["et"] = "est", ["fat"] = "fat", ["fa"] = "fas", ["fj"] = "fij", 
        ["fil"] = "fil", ["fi"] = "fin", ["vls"] = "vls", ["fr"] = "fre", ["fre-02"] = "fre-02", ["fr-be"] = "fre-BE", 
        ["fr-ca"] = "fre-CA", ["fr-fr"] = "fre-FR", ["fr-lu"] = "fre-LU", ["fr-mc"] = "fre-MC", ["fr-ma"] = "fre-MA", 
        ["fr-ch"] = "fre-CH", ["fy"] = "fry", ["ff"] = "ful", ["gd"] = "gla", ["gl"] = "glg", ["ka"] = "kat", 
        ["de"] = "ger", ["de-at"] = "ger-AT", ["de-de"] = "ger-DE", ["de-li"] = "ger-LI", ["de-lu"] = "ger-LU", 
        ["de-ch"] = "ger-CH", ["el"] = "gre", ["kl"] = "kal", ["gn"] = "grn", ["gu"] = "guj", ["ht"] = "hat", 
        ["ha"] = "hau", ["haw"] = "haw", ["haz"] = "haz", ["he"] = "heb", ["hil"] = "hil", ["hi"] = "hin", 
        ["hmn"] = "hmn", ["hu"] = "hun", ["is"] = "ice", ["ig"] = "ibo", ["ilo"] = "ilo", ["id"] = "ind", 
        ["ga"] = "gle", ["it"] = "ita", ["it-it"] = "ita-IT", ["it-ch"] = "ita-CH", ["ja"] = "jpn", ["jv"] = "jav", 
        ["kea"] = "kea", ["kn"] = "kan", ["ksw"] = "ksw", ["ks"] = "kas", ["kyu"] = "kyu", 
        ["eky"] = "eky", ["kk"] = "kaz", ["km"] = "khm", ["gil"] = "gil", ["qqq"] = "qqq", ["ko"] = "kor", 
        ["ckb"] = "ckb", ["kmr"] = "kmr", ["ku"] = "kur", ["kz"] = "kir", ["lo"] = "lao", ["la"] = "lat", 
        ["lv"] = "lav", ["ln"] = "lin", ["lt"] = "lit", ["lb"] = "ltz", ["ymm"] = "ymm", ["mk"] = "mac", ["mg"] = "mlg", 
        ["ms"] = "msa", ["ml"] = "mal", ["mt"] = "mlt", ["mno"] = "mno", ["mnk"] = "mnk", ["mi"] = "mri", 
        ["mr"] = "mar", ["mh"] = "mah", ["fit"] = "fit", ["mo"] = "mol", ["mnw"] = "mnw", ["mn-mn"] = "khk", 
        ["cgy"] = "cgy", ["cgl"] = "cgl", ["na"] = "nau", ["nv"] = "nav", ["ne"] = "nep", ["no"] = "nor", 
        ["nb"] = "nnb", ["nn"] = "nno", ["oc"] = "oci", ["or"] = "ori", ["om"] = "orm", ["ps"] = "pbu", ["pdc"] = "pdc", 
        ["pis"] = "pis", ["pon"] = "pon", ["pl"] = "pol", ["pt"] = "por", ["pt-br"] = "por-BR", ["pt-pt"] = "por-PT", 
        ["pa"] = "pan", ["pnb"] = "pnb", ["qu"] = "quz", ["rki"] = "rki", ["rhg"] = "rhg", ["ro"] = "rum", 
        ["rn"] = "run", ["ru"] = "rus", ["rw"] = "kin", ["sm"] = "smo", ["sa"] = "san", ["scc"] = "scc", 
        ["sr-cyrl-rs"] = "scc", ["sr-cyrl"] = "scc", ["scr"] = "scr", ["sr-latn-rs"] = "scr", ["sr-latn"] = "scr", 
        ["st"] = "sot", ["shn"] = "shn", ["si"] = "sin", ["sk"] = "slo", ["sl"] = "slv", ["so"] = "som",
        ["som-DJ"] = "som-DJ", ["so-dj"] = "som-DJ", ["so-et"] = "som-ET", ["som-ET"] = "som-ET", ["so-ke"] = "som-KE", 
        ["som-KE"] = "som-KE", ["som-SO"] = "som-SO", ["so-so"] = "som-SO", ["es"] = "spa", ["es-ar"] = "spa-AR", 
        ["es-bo"] = "spa-BO", ["es-cl"] = "spa-CL", ["es-co"] = "spa-CO", ["es-cr"] = "spa-CR", ["es-do"] = "spa-DO", 
        ["es-ec"] = "spa-EC", ["es-sv"] = "spa-SV", ["es-gt"] = "spa-GT", ["es-hn"] = "spa-HN", ["spa-M9"] = "spa-M9", 
        ["es-419"] = "spa-M9", ["es-mx"] = "spa-MX", ["es-ni"] = "spa-NI", ["es-pa"] = "spa-PA", ["es-py"] = "spa-PY",
        ["es-pe"] = "spa-PE", ["es-pr"] = "spa-PR", ["es-es"] = "spa-ES", ["es-us"] = "spa-US", ["es-uy"] = "spa-UY", 
        ["es-ve"] = "spa-VE", ["pga"] = "pga", ["su"] = "sun", ["sw"] = "swa", ["sv"] = "swe", ["sv-fi"] = "swe-FI", 
        ["sv-se"] = "swe-SE", ["tl"] = "tgl", ["tg"] = "tgk", ["tgk"] = "tgk", ["tzm"] = "tzm", ["ta"] = "tam",
        ["tt"] = "tat", ["te"] = "tel", ["tdt"] = "tdt", ["th"] = "tha", ["ti"] = "tir", ["tir"] = "tir", 
        ["tpi"] = "tpi", ["to"] = "ton", ["tcs"] = "tcs", ["tn"] = "tsn", ["tr"] = "tur", ["tuk"] = "tuk", 
        ["tk"] = "tuk", ["tvl"] = "tvl", ["tw"] = "twi", ["uk"] = "ukr", ["uk-ua"] = "ukr", ["ur"] = "urd", 
        ["uzn"] = "uzn", ["uz-cyrl"] = "uzn", ["uz-cyrl-uz"] = "uzn", ["uzb"] = "uzb", ["uz-latn"] = "uzb", 
        ["uz-latn-uz"] = "uzb", ["vi"] = "vie", ["cy"] = "wel", ["wo"] = "wol", ["xh"] = "xho", ["yi"] = "yid", 
        ["yo"] = "yor", ["zu"] = "zul"
    };

    [Action("Import glossary", Description = "Import a termbase")]
    public async Task<ImportTermbaseResponse> ImportTermbase([ActionParameter] GlossaryWrapper glossaryWrapper, 
        [ActionParameter] CreateTermbaseRequest input)
    {
        string[] GenerateCsvHeaders(string[] languages)
        {
            var headers = new string[languages.Length * 4];
        
            for (var i = 0; i < languages.Length; i++)
            {
                var language = _csvFileLanguages[languages[i]];
                var index = i * 4;

                headers[index] = $"{language}_{Definition}";
                headers[index + 1] = language;
                headers[index + 2] = $"{TermInfo}-{language}";
                headers[index + 3] = $"{TermExample}-{language}";
            }

            return headers;
        }
        
        string? GetColumnValue(string columnName, GlossaryConceptEntry entry, string languageCode)
        {
            var languageSection = entry.LanguageSections.FirstOrDefault(ls => ls.LanguageCode == languageCode);
            var languageName = _csvFileLanguages[_tbxMemoQLanguages[languageCode]];

            if (languageSection != null)
            {
                switch (columnName)
                {
                    case var name when name == $"{languageName}_{Definition}":
                        return entry.Definition ?? string.Empty;
                    
                    case var name when name == languageName:
                        return languageSection.Terms.First().Term;
                    
                    case var name when name == $"{TermInfo}-{languageName}":
                        var note = string.Join('|', languageSection.Terms.First().Notes ?? Enumerable.Empty<string>())
                            .Replace("\n", "").Replace("\r", "");
                        return note;
                    
                    case var name when name == $"{TermExample}-{languageName}":
                        return string.Empty;
                    
                    default:
                        return null;
                }
            }
            
            if (columnName == $"{languageName}_{Definition}" 
                || columnName == languageName 
                || columnName == $"{TermInfo}-{languageName}" 
                || columnName == $"{TermExample}-{languageName}")
                return string.Empty;

            return null;
        }
        
        await using var glossaryStream = await _fileManagementClient.DownloadAsync(glossaryWrapper.Glossary);
        var glossary = await glossaryStream.ConvertFromTBX();

        var languagesPresent = glossary.ConceptEntries
            .SelectMany(entry => entry.LanguageSections)
            .Select(section => section.LanguageCode)
            .Distinct()
            .ToArray();

        var memoQLanguagesPresent = languagesPresent
            .Select(language => _tbxMemoQLanguages[language])
            .ToArray();

        using var tbService = new MemoqServiceFactory<ITBService>(SoapConstants.TermBasesServiceUrl, Creds);

        var termbaseName = input.Name ?? glossary.Title;
        
        var termbases = await tbService.Service.ListTBs2Async(new TBFilter());
        
        if (termbases.Any(tb => tb.Name.Equals(termbaseName, StringComparison.OrdinalIgnoreCase)))
            termbaseName += $" {DateTime.Now.ToString("D")}";
        
        var termbaseGuid = await tbService.Service.CreateAndPublishAsync(new TBInfo
        {
            IsQTerm = input.IsQTerm ?? false,
            Name = termbaseName,
            Description = input.Description ?? glossary.SourceDescription,
            LanguageCodes = memoQLanguagesPresent,
            IsModerated = input.IsModerated ?? false,
            ModLateDisclosure = input.ModLateDisclosure ?? true,
            Client = input.Client,
            Project = input.Project,
            Domain = input.Domain,
            Subject = input.Subject
        });

        var languageRelatedColumns = GenerateCsvHeaders(memoQLanguagesPresent);
        
        var rowsToAdd = new List<List<string>>();
        rowsToAdd.Add(new List<string>(new[]
        {
            EntryId, EntrySubject, EntryDomain, EntryClientId, EntryProjectId, EntryCreated, EntryCreator,
            EntryLastModified, EntryModifier, EntryNote
        }.Concat(languageRelatedColumns)));
        
        foreach (var entry in glossary.ConceptEntries)
        {
            var languageRelatedValues = (IEnumerable<string>)languagesPresent
                .SelectMany(languageCode =>
                    languageRelatedColumns
                        .Select(column => GetColumnValue(column, entry, languageCode)))
                .Where(value => value != null);
            
            rowsToAdd.Add(new List<string>(new[]
            {
                string.IsNullOrWhiteSpace(entry.Id) ? Guid.NewGuid().ToString() : entry.Id,
                entry.SubjectField ?? string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Join('|', entry.Notes ?? Enumerable.Empty<string>()).Replace("\n", "").Replace("\r", "")
            }.Concat(languageRelatedValues)));
        }

        for (var i = 0; i < rowsToAdd[0].Count; i++)
        {
            var header = rowsToAdd[0][i];
            
            if (header.StartsWith(TermInfo) || header.StartsWith(TermExample))
                rowsToAdd[0][i] = header.Split('-')[0];
        }
        
        await using var csvStream = await rowsToAdd.ConvertToCsv(Encoding.UTF8, ';');
        
        var sessionId = await tbService.Service.BeginChunkedCSVImportAsync(termbaseGuid, new CSVImportSettings());
        try
        {
            const int chunkSize = 500000;

            int bytesRead;
            var buffer = new byte[chunkSize];

            while ((bytesRead = csvStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                var chunk = new byte[bytesRead];
                Array.Copy(buffer, chunk, bytesRead);
                
                await tbService.Service.AddNextCSVChunkAsync(sessionId, chunk);
            }
        }
        finally
        {
            await tbService.Service.EndChunkedCSVImportAsync(sessionId);
        }

        return new() { TermbaseGuid = termbaseGuid.ToString() };
    }

    #endregion

    #region Export

    [Action("Export glossary", Description = "Export a termbase")]
    public async Task<GlossaryWrapper> ExportTermbase([ActionParameter] TermbaseRequest termbaseRequest,
        [ActionParameter] [Display("Include forbidden terms")] bool? includeForbiddenTerms,
        [ActionParameter] [Display("Title")] string? title,
        [ActionParameter] [Display("Description")] string? description)
    {
        using var tbService = new MemoqServiceFactory<ITBService>(SoapConstants.TermBasesServiceUrl, Creds);
        var termbaseGuid = new Guid(termbaseRequest.TermbaseId);
        var termbase = await tbService.Service.GetTBInfoAsync(termbaseGuid);

        var xmlFileBytes = new List<byte>();
        var sessionId = (await tbService.Service.BeginChunkedMultiTermExportAsync(new() { tbGuid = termbaseGuid }))
            .BeginChunkedMultiTermExportResult;
        
        try
        {
            var chunk = await tbService.Service.GetNextExportChunkAsync(sessionId);

            while (chunk != null && chunk.Length != 0)
            {
                xmlFileBytes.AddRange(chunk);
                chunk = await tbService.Service.GetNextExportChunkAsync(sessionId);
            }

            var glossary = ConvertXmlTermbaseToGlossary(xmlFileBytes.ToArray(), includeForbiddenTerms ?? false,
                title ?? termbase.Name, description ?? termbase.Description);
            var glossaryStream = glossary.ConvertToTBX();

            var glossaryFileReference =
                await _fileManagementClient.UploadAsync(glossaryStream, MediaTypeNames.Text.Xml,
                    $"{termbase.Name}.tbx");
            
            return new() { Glossary = glossaryFileReference };
        }
        finally
        {
            await tbService.Service.EndChunkedExportAsync(sessionId);
        }
    }

    private Glossary ConvertXmlTermbaseToGlossary(byte[] xmlBytes, bool includeForbiddenTerms, string termbaseTitle, 
        string? termbaseDescription)
    {
        var xmlContent = Encoding.Unicode.GetString(xmlBytes);
        
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xmlContent);

        var conceptEntries = new List<GlossaryConceptEntry>();
        var glossary = new Glossary(conceptEntries);
        glossary.Title = termbaseTitle;
        glossary.SourceDescription = termbaseDescription;

        var conceptGroupNodes = xmlDocument.SelectNodes("//mtf/conceptGrp")!;

        foreach (XmlElement conceptNode in conceptGroupNodes)
        {
            var languageGroupNodes = conceptNode.SelectNodes("languageGrp")!;
            var languageSections = new List<GlossaryLanguageSection>();

            foreach (XmlElement languageNode in languageGroupNodes)
            {
                if (!includeForbiddenTerms)
                {
                    var termStatus = languageNode
                        .SelectNodes("termGrp/descripGrp/descrip")?
                        .Cast<XmlElement>()
                        .FirstOrDefault(descriptionNode => descriptionNode.Attributes["type"]?.Value == "Status")?
                        .InnerText;
                    
                    if (termStatus == "Forbidden")
                        continue;
                }
                
                var language = languageNode!.SelectSingleNode("language")!.Attributes!["lang"]!.Value.ToLower();
                var term = languageNode.SelectSingleNode("termGrp/term")!.InnerText;
                var termSection = new GlossaryTermSection(term);
                languageSections.Add(new(language, new List<GlossaryTermSection> { termSection }));
            }

            if (languageSections.Any())
            {
                var conceptDescriptionNodes = conceptNode
                    .SelectNodes("descripGrp/descrip")!
                    .Cast<XmlElement>()
                    .ToArray();
                var id = conceptDescriptionNodes
                    .First(descriptionNode => descriptionNode.Attributes["type"]?.Value == "ID").InnerText;
                var subject = conceptDescriptionNodes
                    .FirstOrDefault(descriptionNode => descriptionNode.Attributes["type"]?.Value == "Subject")
                    ?.InnerText;
                var note = conceptDescriptionNodes
                    .FirstOrDefault(descriptionNode => descriptionNode.Attributes["type"]?.Value == "Note")?.InnerText;
                
                var definition = languageGroupNodes.Cast<XmlElement>()
                    .FirstOrDefault(node =>
                    {
                        var descriptionWithDefinition = node
                            .SelectNodes("descripGrp/descrip")?
                            .Cast<XmlElement>()
                            .FirstOrDefault(descriptionNode => descriptionNode.Attributes["type"]?.Value == "Definition");

                        if (descriptionWithDefinition == null)
                            return false;

                        return true;
                    })?
                    .SelectSingleNode("descripGrp/descrip")!.InnerText;
                
                var entry = new GlossaryConceptEntry(id, languageSections)
                {
                    SubjectField = subject,
                    Definition = definition,
                    Notes = new List<string> { note }
                };
            
                conceptEntries.Add(entry);
            }
        }

        return glossary;
    }


    [Action("Update existing glossary", Description ="Update an existing termbase with a new")]    
    public async Task UpdateExistingGlossary([ActionParameter] GlossaryWrapper glossaryWrapper,
        [ActionParameter] UpdateExistingTermbaseRequest input)
    {
        await using var glossaryStream = await _fileManagementClient.DownloadAsync(glossaryWrapper.Glossary);

        var csvImportSettings = new CSVImportIntoExistingSettings
        {
            AllowAddNewLanguages = input.AllowAddNewLanguages ?? true,
            OverwriteEntriesWithSameId = input.OverwriteEntiesWithSameId ?? false,
            Delimiter = ';'
        };

        using var tbService = new MemoqServiceFactory<ITBService>(SoapConstants.TermBasesServiceUrl, Creds);

        var tbGuid = Guid.Parse(input.Id);
        var fileGuid = Guid.NewGuid();

        var taskInfo = await tbService.Service.StartCSVImportIntoExistingTBTaskAsync(fileGuid, tbGuid, csvImportSettings);
        var sessionId = taskInfo.TaskId;


        try
        {
            const int chunkSize = 500000;
            var buffer = new byte[chunkSize];
            int bytesRead;

            while((bytesRead = await glossaryStream.ReadAsync(buffer, 0, buffer.Length))>0)
            {
                var chunk = new byte[bytesRead];
                Array.Copy(buffer, chunk, bytesRead);
                await tbService.Service.AddNextCSVChunkAsync(sessionId, chunk);
            }

            await tbService.Service.EndChunkedCSVImportAsync(sessionId);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            try
            {
                await tbService.Service.EndChunkedCSVImportAsync(sessionId);
            }
            catch (Exception finalEx)
            {
                Console.WriteLine($"Failed to finalize the import session: {finalEx.Message}");
            }
        }
    }

    


    #endregion
}