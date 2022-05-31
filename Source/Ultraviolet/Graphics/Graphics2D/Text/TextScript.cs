namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents the scripts which are recognized by the Ultraviolet layout engine.
    /// </summary>
    public enum TextScript
    {
        /// <summary>
        /// An invalid script.
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// The common script, which contains characters that are not part of a formal natural language writing system,
        /// such as currency symbols, numerals, and punctuation marks.
        /// </summary>
        Common,

        /// <summary>
        /// The inherited script, which contains characters that are combined with other characters and which
        /// inherit their script value from those characters.
        /// </summary>
        Inherited,

        /// <summary>
        /// The unknown script, which contains unassigned, private use, noncharacter, and surrogate code points.
        /// </summary>
        Unknown,

        /// <summary>
        /// The Arabic script.
        /// </summary>
        Arabic,

        /// <summary>
        /// The Armenian script.
        /// </summary>
        Armenian,

        /// <summary>
        /// The Bengali script.
        /// </summary>
        Bengali,

        /// <summary>
        /// The Cyrillic script.
        /// </summary>
        Cyrillic,

        /// <summary>
        /// The Devanagari script.
        /// </summary>
        Devanagari,

        /// <summary>
        /// The Georgian script.
        /// </summary>
        Georgian,

        /// <summary>
        /// The Greek script.
        /// </summary>
        Greek,

        /// <summary>
        /// The Gujarati script.
        /// </summary>
        Gujarati,

        /// <summary>
        /// The Gurmukhi script.
        /// </summary>
        Gurmukhi,

        /// <summary>
        /// The Hangul script.
        /// </summary>
        Hangul,

        /// <summary>
        /// The Han script.
        /// </summary>
        Han,

        /// <summary>
        /// The Hebrew script.
        /// </summary>
        Hebrew,

        /// <summary>
        /// The Hiragana script.
        /// </summary>
        Hiragana,

        /// <summary>
        /// The Kannada script.
        /// </summary>
        Kannada,

        /// <summary>
        /// The Katakana script.
        /// </summary>
        Katakana,

        /// <summary>
        /// The Lao script.
        /// </summary>
        Lao,

        /// <summary>
        /// The Latin script.
        /// </summary>
        Latin,

        /// <summary>
        /// The Malayalam script.
        /// </summary>
        Malayalam,

        /// <summary>
        /// The Oriya script.
        /// </summary>
        Oriya,

        /// <summary>
        /// The Tamil script.
        /// </summary>
        Tamil,

        /// <summary>
        /// The Telugu script.
        /// </summary>
        Telugu,

        /// <summary>
        /// The Thai script.
        /// </summary>
        Thai,

        /// <summary>
        /// The Tibetan script.
        /// </summary>
        Tibetan,

        /// <summary>
        /// The Bopomofo script.
        /// </summary>
        Bopomofo,

        /// <summary>
        /// The Braille script.
        /// </summary>
        Braille,

        /// <summary>
        /// The Canadian Syllabics script.
        /// </summary>
        CanadianSyllabics,

        /// <summary>
        /// The Cherokee script.
        /// </summary>
        Cherokee,

        /// <summary>
        /// The Ethiopic script.
        /// </summary>
        Ethiopic,

        /// <summary>
        /// The Khmer script.
        /// </summary>
        Khmer,

        /// <summary>
        /// The Mongolian script.
        /// </summary>
        Mongolian,

        /// <summary>
        /// The Myanmar script.
        /// </summary>
        Myanmar,

        /// <summary>
        /// The Ogham script.
        /// </summary>
        Ogham,

        /// <summary>
        /// The Runic script.
        /// </summary>
        Runic,

        /// <summary>
        /// The Sinhala script.
        /// </summary>
        Sinhala,

        /// <summary>
        /// The Syriac script.
        /// </summary>
        Syriac,

        /// <summary>
        /// The Thaana script.
        /// </summary>
        Thaana,

        /// <summary>
        /// The Yi script.
        /// </summary>
        Yi,

        /// <summary>
        /// The Deseret script.
        /// </summary>
        Deseret,

        /// <summary>
        /// The Gothic script.
        /// </summary>
        Gothic,

        /// <summary>
        /// The Old Italic script.
        /// </summary>
        OldItalic,

        /// <summary>
        /// The Buhid script.
        /// </summary>
        Buhid,

        /// <summary>
        /// The Hanunoo script.
        /// </summary>
        Hanunoo,

        /// <summary>
        /// The Tagalog script.
        /// </summary>
        Tagalog,

        /// <summary>
        /// The Tagbanwa script.
        /// </summary>
        Tagbanwa,

        /// <summary>
        /// The Cypriot script.
        /// </summary>
        Cypriot,

        /// <summary>
        /// The Limbu script.
        /// </summary>
        Limbu,

        /// <summary>
        /// The Linear B script.
        /// </summary>
        LinearB,

        /// <summary>
        /// The Osmanya script.
        /// </summary>
        Osmanya,

        /// <summary>
        /// The Shavian script.
        /// </summary>
        Shavian,

        /// <summary>
        /// The Tai Le script.
        /// </summary>
        TaiLe,

        /// <summary>
        /// The Ugaritic script.
        /// </summary>
        Ugaritic,

        /// <summary>
        /// The Buginese script.
        /// </summary>
        Buginese,

        /// <summary>
        /// The Coptic script.
        /// </summary>
        Coptic,

        /// <summary>
        /// The Glagolitic script.
        /// </summary>
        Glagolitic,

        /// <summary>
        /// The Kharoshthi script.
        /// </summary>
        Kharoshthi,

        /// <summary>
        /// The New Tai Lue script.
        /// </summary>
        NewTaiLue,

        /// <summary>
        /// The Old Persian script.
        /// </summary>
        OldPersian,

        /// <summary>
        /// The Syloti Nagri script.
        /// </summary>
        SylotiNagri,

        /// <summary>
        /// The Tiginagh script.
        /// </summary>
        Tifinagh,

        /// <summary>
        /// The Balinese script.
        /// </summary>
        Balinese,

        /// <summary>
        /// The Cuneiform script.
        /// </summary>
        Cuneiform,

        /// <summary>
        /// The Nko script.
        /// </summary>
        Nko,

        /// <summary>
        /// The Phags Pa script.
        /// </summary>
        PhagsPa,

        /// <summary>
        /// The Phoenician script.
        /// </summary>
        Phoenician,

        /// <summary>
        /// The Carian script.
        /// </summary>
        Carian,

        /// <summary>
        /// The Cham script.
        /// </summary>
        Cham,

        /// <summary>
        /// The Kayah Li script.
        /// </summary>
        KayahLi,

        /// <summary>
        /// The Lepcha script.
        /// </summary>
        Lepcha,

        /// <summary>
        /// The Lycian script.
        /// </summary>
        Lycian,

        /// <summary>
        /// The Lydian script.
        /// </summary>
        Lydian,

        /// <summary>
        /// The Ol Chiki script.
        /// </summary>
        OlChiki,

        /// <summary>
        /// The Rejang script.
        /// </summary>
        Rejang,

        /// <summary>
        /// The Saurashtra script.
        /// </summary>
        Saurashtra,

        /// <summary>
        /// The Sundanese script.
        /// </summary>
        Sundanese,

        /// <summary>
        /// The Vai script.
        /// </summary>
        Vai,

        /// <summary>
        /// The Avestan script.
        /// </summary>
        Avestan,

        /// <summary>
        /// The Bamum script.
        /// </summary>
        Bamum,

        /// <summary>
        /// The Egyptian Hieroglyphs script.
        /// </summary>
        EgyptianHieroglyphs,

        /// <summary>
        /// The Imperial Aramaic script.
        /// </summary>
        ImperialAramaic,

        /// <summary>
        /// The Inscriptional Pahlavi script.
        /// </summary>
        InscriptionalPahlavi,

        /// <summary>
        /// The Inscriptional Parthian script.
        /// </summary>
        InscriptionalParthian,

        /// <summary>
        /// The Javanese script.
        /// </summary>
        Javanese,

        /// <summary>
        /// The Kaithi script.
        /// </summary>
        Kaithi,

        /// <summary>
        /// The Lisu script.
        /// </summary>
        Lisu,

        /// <summary>
        /// The Meetei Mayek script.
        /// </summary>
        MeeteiMayek,

        /// <summary>
        /// The Old South Arabian script.
        /// </summary>
        OldSouthArabian,

        /// <summary>
        /// The Old Turkic script.
        /// </summary>
        OldTurkic,

        /// <summary>
        /// The Samaritan script.
        /// </summary>
        Samaritan,

        /// <summary>
        /// The Tai Tham script.
        /// </summary>
        TaiTham,

        /// <summary>
        /// The Tai Viet script.
        /// </summary>
        TaiViet,

        /// <summary>
        /// The Batak script.
        /// </summary>
        Batak,

        /// <summary>
        /// The Brahmi script.
        /// </summary>
        Brahmi,

        /// <summary>
        /// The Mandaic script.
        /// </summary>
        Mandaic,

        /// <summary>
        /// The Chakma script.
        /// </summary>
        Chakma,

        /// <summary>
        /// The Meroitic Cursive script.
        /// </summary>
        MeroiticCursive,

        /// <summary>
        /// The Meroitic Hieroglyphs script.
        /// </summary>
        MeroiticHieroglyphs,

        /// <summary>
        /// The Miao script.
        /// </summary>
        Miao,

        /// <summary>
        /// The Sharada script.
        /// </summary>
        Sharada,

        /// <summary>
        /// The Sora Sompeng script.
        /// </summary>
        SoraSompeng,

        /// <summary>
        /// The Takir script.
        /// </summary>
        Takri,

        /// <summary>
        /// The Bassa Vah script.
        /// </summary>
        BassaVah,

        /// <summary>
        /// The Caucasian Albanian script.
        /// </summary>
        CaucasianAlbanian,

        /// <summary>
        /// The Duployan script.
        /// </summary>
        Duployan,

        /// <summary>
        /// The Elbasan script.
        /// </summary>
        Elbasan,

        /// <summary>
        /// The Grantha script.
        /// </summary>
        Grantha,

        /// <summary>
        /// The Khojki script.
        /// </summary>
        Khojki,

        /// <summary>
        /// The Khudawadi script.
        /// </summary>
        Khudawadi,

        /// <summary>
        /// The Linear A script.
        /// </summary>
        LinearA,

        /// <summary>
        /// The Mahajani script.
        /// </summary>
        Mahajani,

        /// <summary>
        /// The Manichaean script.
        /// </summary>
        Manichaean,

        /// <summary>
        /// The Mende Kikakui script.
        /// </summary>
        MendeKikakui,

        /// <summary>
        /// The Modi script.
        /// </summary>
        Modi,

        /// <summary>
        /// The Mro script.
        /// </summary>
        Mro,

        /// <summary>
        /// The Nabataean script.
        /// </summary>
        Nabataean,

        /// <summary>
        /// The Old North Arabian script.
        /// </summary>
        OldNorthArabian,

        /// <summary>
        /// The Old Permic script.
        /// </summary>
        OldPermic,

        /// <summary>
        /// The Pahawh Hmong script.
        /// </summary>
        PahawhHmong,

        /// <summary>
        /// The Palmyrene script.
        /// </summary>
        Palmyrene,

        /// <summary>
        /// The Pau Cin Hau script.
        /// </summary>
        PauCinHau,

        /// <summary>
        /// The Psalter Pahlavi script.
        /// </summary>
        PsalterPahlavi,

        /// <summary>
        /// The Siddham script.
        /// </summary>
        Siddham,

        /// <summary>
        /// The Tirhuta script.
        /// </summary>
        Tirhuta,

        /// <summary>
        /// The Warang Citi script.
        /// </summary>
        WarangCiti,

        /// <summary>
        /// The Ahom script.
        /// </summary>
        Ahom,

        /// <summary>
        /// The Anatolian Hieroglyphs script.
        /// </summary>
        AnatolianHieroglyphs,

        /// <summary>
        /// The Hatran script.
        /// </summary>
        Hatran,

        /// <summary>
        /// The Multani script.
        /// </summary>
        Multani,

        /// <summary>
        /// The Old Hungarian script.
        /// </summary>
        OldHungarian,
        
        /// <summary>
        /// The SignWriting script.
        /// </summary>
        SignWriting,

        /// <summary>
        /// The Adlam script.
        /// </summary>
        Adlam,

        /// <summary>
        /// The Bhaiksuki script.
        /// </summary>
        Bhaiksuki,

        /// <summary>
        /// The Marchen script.
        /// </summary>
        Marchen,

        /// <summary>
        /// The Osage script.
        /// </summary>
        Osage,

        /// <summary>
        /// The Tangut script.
        /// </summary>
        Tangut,

        /// <summary>
        /// The Newa script.
        /// </summary>
        Newa,

        /// <summary>
        /// The Masaram Gondi script.
        /// </summary>
        MasaramGondi,

        /// <summary>
        /// The Nushu script.
        /// </summary>
        Nushu,

        /// <summary>
        /// The Soyombo script.
        /// </summary>
        Soyombo,

        /// <summary>
        /// The Zanbazar Square script.
        /// </summary>
        ZanabazarSquare,
    }
}
