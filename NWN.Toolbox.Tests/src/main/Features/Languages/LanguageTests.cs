using System.Collections.Generic;
using System.Linq;
using Anvil.Services;
using Jorteck.Toolbox.Features.Languages;
using NUnit.Framework;

namespace Jorteck.Toolbox.Tests.Features.Languages
{
  [TestFixture(Category = "Features.Languages")]
  public sealed class LanguageTests
  {
    [Inject]
    private static IReadOnlyList<ILanguage> Languages { get; set; }

    [Test(Description = "Translating the pangram \"the five boxing wizards jump quickly.\" with a given language returns the expected output.")]
    [TestCaseSource(nameof(LanguageTranslationCases))]
    public void LanguageReturnsExpectedTranslation(ILanguage language, string expectedOutputLower, string expectedOutputMixed, string expectedOutputUpper)
    {
      string inputLower = "the five boxing wizards jump quickly.";
      string inputMixed = "The Five Boxing Wizards Jump Quickly.";
      string inputUpper = "THE FIVE BOXING WIZARDS JUMP QUICKLY.";

      LanguageOutput outputLower = language.Translate(inputLower, LanguageProficiency.Fluent);
      LanguageOutput outputMixed = language.Translate(inputMixed, LanguageProficiency.Fluent);
      LanguageOutput outputUpper = language.Translate(inputUpper, LanguageProficiency.Fluent);

      Assert.That(outputLower.Output, Is.EqualTo(expectedOutputLower));
      Assert.That(outputMixed.Output, Is.EqualTo(expectedOutputMixed));
      Assert.That(outputUpper.Output, Is.EqualTo(expectedOutputUpper));
    }

    private static object[] LanguageTranslationCases()
    {
      return new object[]
      {
        new object[] { Languages.OfType<LanguageAbyssal>().First(), "gda kootsa nebboots bootoongm haepb chaeoomblee.", "Gda Kootsa Nebboots Bootoongm Haepb Chaeoomblee.", "GDA KOOTsA NEBbOOTS BOOTOONGM HAePB ChAeOOMBLEe." },
        new object[] { Languages.OfType<LanguageAnimal>().First(), "''' '''' '''''' ''''''' '''' '''''''.", "''' '''' '''''' ''''''' '''' '''''''.", "''' '''' '''''' ''''''' '''' '''''''." },
        new object[] { Languages.OfType<LanguageCelestial>().First(), "yrel bijel pugicw fikantl mosq doivxhz.", "Yrel Bijel Pugicw Fikantl Mosq Doivxhz.", "YREl BIJEl PUGICW FIKANTL MOSQ DOIVXHZ." },
        new object[] { Languages.OfType<LanguageDraconic>().First(), "drnii wunfii poyquunrak ziunjiehutymy vionliba xonunstgochan.", "Drnii Wunfii Poyquunrak Ziunjiehutymy Vionliba Xonunstgochan.", "DrNiI WUnFiI PoYQuUnRaK ZiUnJiEHuTyMy ViOnLiBa XOnUnStGoChAn." },
        new object[] { Languages.OfType<LanguageDrow>().First(), "anira oeela fe'elv kyep'ilmwla viylity ryestgoca.", "Anira Oeela Fe'elv Kyep'ilmwla Viylity Ryestgoca.", "AnIrA OEElA FE'ELV KyEP'IlMWLa ViYLiTy RYEStGoCA." },
        new object[] { Languages.OfType<LanguageDwarven>().First(), "k'a waaga pourqark zhajazhtth dr'lrh k'azigno.", "K'a Waaga Pourqark Zhajazhtth Dr'lrh K'azigno.", "K'A WaAGA PoUrQARK ZhAJAzHTTh Dr'LRh K'AZiGNO." },
        new object[] { Languages.OfType<LanguageElven>().First(), "anira oeela fe'elv amejilmwla quysty hyenynca.", "Anira Oeela Fe'elv Amejilmwla Quysty Hyenynca.", "AnIrA OEElA FE'ELV AmEJIlMWLa QuYSTy HYENyNCA." },
        new object[] { Languages.OfType<LanguageGnome>().First(), "dra veja puqehk fewyntc zisb xielgmo.", "Dra Veja Puqehk Fewyntc Zisb Xielgmo.", "DRA VEJA PUQEHK FEWYNTC ZISB XIELGMO." },
        new object[] { Languages.OfType<LanguageGoblin>().First(), "dr' vo' puok 'owuntk zusb uogmo.", "Dr' Vo' Puok 'owuntk Zusb uogmo.", "DR' VO' PUOK 'OWUNTK ZUSB UOGMO." },
        new object[] { Languages.OfType<LanguageHalfling>().First(), "dni wufi pyqurk zujehtm volb xousgca.", "Dni Wufi Pyqurk Zujehtm Volb Xousgca.", "DNI WUFI PYQURK ZUJEHTM VOLB XOUSGCA." },
        new object[] { Languages.OfType<LanguageInfernal>().First(), "dra vyra cykyrk 'ygonjk z'zk r'yrgmi.", "Dra Vyra Cykyrk 'ygonjk Z'zk R'yrgmi.", "DRA VYRA CYKYRK 'YGONJK Z'ZK R'YRGMI." },
        new object[] { Languages.OfType<LanguageMulhorandi>().First(), "sua joza djehquomy pozrishalth fetk ngeopchdo.", "Sua Joza Djehquomy Pozrishalth Fetk Ngeopchdo.", "SUA JOZA DjEhQuOMY POZRiShAlTh FETK NgEOPChDO." },
        new object[] { Languages.OfType<LanguageOrc>().First(), "nro ago purakk ramhahtg mrb kazgh'.", "Nro ago Purakk Ramhahtg Mrb Kazgh'.", "NRO AGO PURAKK RAMHaHTG MRB KAZGH'." },
        new object[] { Languages.OfType<LanguageRashemi>().First(), "kafrov dukaov sooussk mauya yor oozj skounfroj.", "Kafrov Dukaov Sooussk Mauya yor Oozj Skounfroj.", "KaFrOv DUKaOv SOOUSSk MaUYAMYOr OOZJ SkOUNFROj." },
        new object[] { Languages.OfType<LanguageSylvan>().First(), "nafi moyavi rinuyayajyo niyaogiwmane talirwi boliyabamtmi.", "Nafi Moyavi Rinuyayajyo Niyaogiwmane Talirwi Boliyabamtmi.", "NaFI MoYaVI RiNuYaYaJYo NiYaOgIWMaNe TaLiRWi BoLiYaBaMTMi." },
        new object[] { Languages.OfType<LanguageThievesCant>().First(), "*turns a bit*", "*turns a bit*", "*turns a bit*" },
      };
    }
  }
}
