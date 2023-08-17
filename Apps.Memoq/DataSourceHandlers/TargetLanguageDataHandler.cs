using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Memoq.DataSourceHandlers;

public class TargetLanguageDataHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        ["afr"] = "Afrikaans", ["aka"] = "Akan", ["alb"] = "Albanian", ["alb-AL"] = "Albanian (Albania)",
        ["alb-XK"] = "Albanian (Kosovo)", ["alb-MK"] = "Albanian (Macedonia)", ["alb-ME"] = "Albanian (Montenegro)",
        ["amh"] = "Amharic", ["ara"] = "Arabic", ["ara-DZ"] = "Arabic (Algeria)", ["ara-BH"] = "Arabic (Bahrain)",
        ["ara-EG"] = "Arabic (Egypt)", ["ara-IQ"] = "Arabic (Iraq)", ["ara-JO"] = "Arabic (Jordan)",
        ["ara-KW"] = "Arabic (Kuwait)", ["ara-LB"] = "Arabic (Lebanon)", ["ara-LY"] = "Arabic (Libya)",
        ["ara-MA"] = "Arabic (Morocco)", ["ara-OM"] = "Arabic (Oman)", ["ara-QA"] = "Arabic (Qatar)",
        ["ara-SA"] = "Arabic (Saudi Arabia)", ["ara-SY"] = "Arabic (Syria)", ["ara-TN"] = "Arabic (Tunisia)",
        ["ara-AE"] = "Arabic (U.A.E.)", ["ara-YE"] = "Arabic (Yemen)", ["arg"] = "Aragonese", ["ocs"] = "Aranese",
        ["hye"] = "Armenian", ["asm"] = "Assamese", ["ast"] = "Asturian", ["azf"] = "Azeri (Cyrillic)",
        ["aze"] = "Azeri (Latin)", ["bxg"] = "Bangala", ["baq"] = "Basque", ["bel"] = "Belarussian",
        ["ben"] = "Bengali", ["ben-BD"] = "Bengali (Bangladesh)", ["ben-IN"] = "Bengali (India)", ["bis"] = "Bislama",
        ["boc"] = "Bosnian (Cyrillic)", ["bos"] = "Bosnian (Latin)", ["bre"] = "Breton", ["bul"] = "Bulgarian",
        ["mya"] = "Burmese", ["cat"] = "Catalan", ["ceb"] = "Cebuano", ["chr"] = "Cherokee", ["ctd"] = "Chin",
        ["zho-HK"] = "Chinese (Hong Kong S.A.R.)", ["zho-MO"] = "Chinese (Macao S.A.R.)", ["zho-CN"] = "Chinese (PRC)",
        ["zho-SG"] = "Chinese (Singapore)", ["zho-TW"] = "Chinese (Taiwan)", ["cho"] = "Choctaw", ["chk"] = "Chuukese",
        ["hrv"] = "Croatian", ["cze"] = "Czech", ["dan"] = "Danish", ["prs"] = "Dari", ["din"] = "Dinka",
        ["dut"] = "Dutch", ["dut-BE"] = "Dutch (Belgium)", ["dut-NL"] = "Dutch (Netherlands)", ["eng"] = "English",
        ["eng-AU"] = "English (Australia)", ["eng-BZ"] = "English (Belize)", ["eng-CA"] = "English (Canada)",
        ["eng-CB"] = "English (Caribbean)", ["eng-IE"] = "English (Ireland)", ["eng-JM"] = "English (Jamaica)",
        ["eng-NZ"] = "English (New Zealand)", ["eng-PH"] = "English (Republic of the Philippines)",
        ["eng-ZA"] = "English (South Africa)", ["eng-TT"] = "English (Trinidad and Tobago)",
        ["eng-GB"] = "English (United Kingdom)", ["eng-US"] = "English (United States)",
        ["eng-ZW"] = "English (Zimbabwe)", ["epo"] = "Esperanto", ["est"] = "Estonian", ["fat"] = "Fanti",
        ["fas"] = "Farsi", ["fij"] = "Fijian", ["fil"] = "Filipino", ["fin"] = "Finnish", ["vls"] = "Flemish",
        ["fre"] = "French", ["fre-02"] = "French (Africa)", ["fre-BE"] = "French (Belgium)",
        ["fre-CA"] = "French (Canada)", ["fre-FR"] = "French (France)", ["fre-LU"] = "French (Luxembourg)",
        ["fre-MC"] = "French (Monaco)", ["fre-MA"] = "French (Morocco)", ["fre-CH"] = "French (Switzerland)",
        ["fry"] = "Frisian, Western", ["ful"] = "Fulah", ["gla"] = "Gaelic (Scotland)", ["glg"] = "Galician",
        ["kat"] = "Georgian", ["ger"] = "German", ["ger-AT"] = "German (Austria)", ["ger-DE"] = "German (Germany)",
        ["ger-LI"] = "German (Liechtenstein)", ["ger-LU"] = "German (Luxembourg)", ["ger-CH"] = "German (Switzerland)",
        ["gre"] = "Greek", ["kal"] = "Greenlandic", ["grn"] = "Guaraní", ["guj"] = "Gujarati",
        ["hat"] = "Haitian Creole", ["hau"] = "Hausa", ["haw"] = "Hawaiian", ["haz"] = "Hazaragi", ["heb"] = "Hebrew",
        ["hil"] = "Hiligaynon", ["hin"] = "Hindi", ["hmn"] = "Hmong", ["hun"] = "Hungarian", ["ice"] = "Icelandic",
        ["ibo"] = "Igbo", ["ilo"] = "Ilocano", ["ind"] = "Indonesian", ["gle"] = "Irish", ["ita"] = "Italian",
        ["ita-IT"] = "Italian (Italy)", ["ita-CH"] = "Italian (Switzerland)", ["jpn"] = "Japanese",
        ["jav"] = "Javanese", ["kea"] = "Kabuverdianu", ["kan"] = "Kannada", ["ksw"] = "Karen", ["kas"] = "Kashmiri",
        ["kyu"] = "Kayah (Latin)", ["eky"] = "Kayah (Myanmar)", ["kaz"] = "Kazakh", ["khm"] = "Khmer",
        ["gil"] = "Kiribati", ["qqq"] = "Klingon", ["kor"] = "Korean", ["ckb"] = "Kurdish (Arabic)",
        ["kmr"] = "Kurdish (Cyrillic)", ["kur"] = "Kurdish (Latin)", ["kir"] = "Kyrgyz (Cyrillic)", ["lao"] = "Lao",
        ["lat"] = "Latin", ["lav"] = "Latvian", ["lin"] = "Lingala", ["lit"] = "Lithuanian", ["ltz"] = "Luxembourgish",
        ["ymm"] = "Maay", ["mac"] = "Macedonian", ["mlg"] = "Malagasy", ["msa"] = "Malay", ["mal"] = "Malayalam",
        ["mlt"] = "Maltese", ["mno"] = "Mandinka (Arabic)", ["mnk"] = "Mandinka (Latin)", ["mri"] = "Maori",
        ["mar"] = "Marathi", ["mah"] = "Marshallese", ["fit"] = "Meänkieli", ["mol"] = "Moldavian", ["mnw"] = "Mon",
        ["khk"] = "Mongolian (Cyrillic)", ["cgy"] = "Montenegrin (Cyrillic)", ["cgl"] = "Montenegrin (Latin)",
        ["nau"] = "Nauruan", ["nav"] = "Navajo", ["nep"] = "Nepali", ["nor"] = "Norwegian",
        ["nnb"] = "Norwegian (Bokmål)", ["nno"] = "Norwegian (Nynorsk)", ["oci"] = "Occitan", ["ori"] = "Oriya",
        ["orm"] = "Oromo", ["pbu"] = "Pashto", ["pdc"] = "Pennsylvania German", ["pis"] = "Pijin",
        ["pon"] = "Pohnpeian", ["pol"] = "Polish", ["por"] = "Portuguese", ["por-BR"] = "Portuguese (Brazil)",
        ["por-PT"] = "Portuguese (Portugal)", ["pan"] = "Punjabi (Gurmukhi)", ["pnb"] = "Punjabi (Shahmukhi)",
        ["quz"] = "Quechua", ["rki"] = "Rakhine", ["rhg"] = "Rohingya", ["rum"] = "Romanian", ["run"] = "Rundi",
        ["rus"] = "Russian", ["kin"] = "Rwanda", ["smo"] = "Samoan", ["san"] = "Sanskrit",
        ["scc"] = "Serbian (Cyrillic)", ["scr"] = "Serbian (Latin)", ["sot"] = "Sesotho", ["shn"] = "Shan",
        ["sin"] = "Sinhala", ["slo"] = "Slovak", ["slv"] = "Slovenian", ["som"] = "Somali",
        ["som-DJ"] = "Somali (Djibouti)", ["som-ET"] = "Somali (Ethiopia)", ["som-KE"] = "Somali (Kenya)",
        ["som-SO"] = "Somali (Somalia)", ["spa"] = "Spanish", ["spa-AR"] = "Spanish (Argentina)",
        ["spa-BO"] = "Spanish (Bolivia)", ["spa-CL"] = "Spanish (Chile)", ["spa-CO"] = "Spanish (Colombia)",
        ["spa-CR"] = "Spanish (Costa Rica)", ["spa-DO"] = "Spanish (Dominican Republic)",
        ["spa-EC"] = "Spanish (Ecuador)", ["spa-SV"] = "Spanish (El Salvador)", ["spa-GT"] = "Spanish (Guatemala)",
        ["spa-HN"] = "Spanish (Honduras)", ["spa-M9"] = "Spanish (Latin America)", ["spa-MX"] = "Spanish (Mexico)",
        ["spa-NI"] = "Spanish (Nicaragua)", ["spa-PA"] = "Spanish (Panama)", ["spa-PY"] = "Spanish (Paraguay)",
        ["spa-PE"] = "Spanish (Peru)", ["spa-PR"] = "Spanish (Puerto Rico)", ["spa-ES"] = "Spanish (Spain)",
        ["spa-US"] = "Spanish (United States)", ["spa-UY"] = "Spanish (Uruguay)", ["spa-VE"] = "Spanish (Venezuela)",
        ["pga"] = "Sudanese Creole Arabic", ["sun"] = "Sundanese", ["swa"] = "Swahili", ["swe"] = "Swedish",
        ["swe-FI"] = "Swedish (Finland)", ["swe-SE"] = "Swedish (Sweden)", ["tgl"] = "Tagalog",
        ["tgk"] = "Tajiki (Cyrillic)", ["tzm"] = "Tamazight", ["tam"] = "Tamil", ["tat"] = "Tatar", ["tel"] = "Telugu",
        ["tdt"] = "Tetun Dili", ["tha"] = "Thai", ["tir"] = "Tigrigna", ["tpi"] = "Tok Pisin", ["ton"] = "Tongan",
        ["tcs"] = "Torres Strait Creole", ["tsn"] = "Tswana", ["tur"] = "Turkish", ["tuk"] = "Turkmen (Latin)",
        ["tvl"] = "Tuvaluan", ["twi"] = "Twi", ["ukr"] = "Ukrainian", ["urd"] = "Urdu", ["uzn"] = "Uzbek (Cyrillic)",
        ["uzb"] = "Uzbek (Latin)", ["vie"] = "Vietnamese", ["wel"] = "Welsh", ["wol"] = "Wolof", ["xho"] = "Xhosa",
        ["yid"] = "Yiddish", ["yor"] = "Yoruba", ["zul"] = "Zulu"
    };
}