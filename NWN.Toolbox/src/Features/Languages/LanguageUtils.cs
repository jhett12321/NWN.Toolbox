using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jorteck.Toolbox.Features.Languages
{
  public static class LanguageUtils
  {
    private static readonly Random Random = new Random();

    public static int GetFluencyFromProficiency(int proficiency)
    {
      return proficiency switch
      {
        >= LanguageProficiency.Fluent => 100,
        >= LanguageProficiency.Advanced => 95,
        >= LanguageProficiency.Intermediate => 80,
        >= LanguageProficiency.Beginner => 60,
        _ => 30,
      };
    }

    public static LanguageOutput TranslateUsingDictionary(Dictionary<char, string> dictionary, string phrase, int proficiency)
    {
      int fluency = GetFluencyFromProficiency(proficiency);

      StringBuilder interpretation = new StringBuilder(phrase.Length);
      StringBuilder output = new StringBuilder();

      foreach (char c in phrase)
      {
        int check = Random.Next(0, 100);
        if (check < fluency)
        {
          if (dictionary.TryGetValue(c, out string mapping))
          {
            output.Append(mapping);
          }
          else
          {
            output.Append(c);
          }

          interpretation.Append(c);
        }
        else
        {
          (char key, string value) = dictionary.ElementAt(Random.Next(0, dictionary.Count));
          interpretation.Append(key);
          output.Append(value);
        }
      }

      return new LanguageOutput
      {
        Interpretation = interpretation.ToString(),
        SpokenText = output.ToString(),
      };
    }
  }
}
