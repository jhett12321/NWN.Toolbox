using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jorteck.Toolbox.Features.Languages
{
  public static class LanguageUtils
  {
    /// <summary>
    /// Generates a translation using the specified character dictionary as a lookup.
    /// </summary>
    /// <param name="language">The language that is generating the translation.</param>
    /// <param name="dictionary">The dictionary to use as a lookup.</param>
    /// <param name="phrase">The phrase to translate.</param>
    /// <param name="proficiency">The character's proficiency with the language.</param>
    /// <returns>The generated language output.</returns>
    public static LanguageOutput TranslateUsingDictionary(ILanguage language, Dictionary<char, string> dictionary, string phrase, int proficiency)
    {
      int fluency = GetFluencyFromProficiency(proficiency);

      StringBuilder interpretation = new StringBuilder(phrase.Length);
      StringBuilder output = new StringBuilder();

      bool escaped = false;
      foreach (char c in phrase)
      {
        if (c == '*')
        {
          escaped = !escaped;
        }

        if (escaped)
        {
          interpretation.Append(c);
          output.Append(c);
          continue;
        }

        int check = Random.Shared.Next(0, 100);
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
          (char key, string value) = dictionary.ElementAt(Random.Shared.Next(0, dictionary.Count));
          interpretation.Append(key);
          output.Append(value);
        }
      }

      return new LanguageOutput(language, interpretation.ToString(), output.ToString());
    }

    /// <summary>
    /// Generates a deterministic translation using the given seed.
    /// </summary>
    /// <param name="language">The language that is generating the translation.</param>
    /// <param name="seed">The seed to use for generating the translation.</param>
    /// <param name="phrase">The phrase to translate.</param>
    /// <param name="proficiency">The character's proficiency with the language.</param>
    /// <returns>The generated language output.</returns>
    public static LanguageOutput TranslateWithSeed(ILanguage language, int seed, string phrase, int proficiency)
    {
      int fluency = GetFluencyFromProficiency(proficiency);

      // Functional groups for translations made with a certain seed.
      // Vowel Sounds: a, e, i, o, u
      // Hard Sounds: b, d, k, p, t
      // Sibilant Sounds: c, f, s, q, w
      // Soft Sounds: g, h, l, r, y
      // Hummed Sounds: j, m, n, v, z
      // Oddball out: x, the rarest letter in the alphabet
      const string cipher = "aeiouAEIOUbdkptBDKPTcfsqwCFSQWghlryGHLRYjmnvzJMNVZxX";

      StringBuilder interpretation = new StringBuilder(phrase.Length);
      StringBuilder output = new StringBuilder();

      bool escaped = false;
      foreach (char c in phrase)
      {
        if (c == '*')
        {
          escaped = !escaped;
        }

        if (escaped)
        {
          interpretation.Append(c);
          output.Append(c);
          continue;
        }

        int check = Random.Shared.Next(0, 100);
        if (check < fluency)
        {
          int index = cipher.IndexOf(c);
          if (index != -1)
          {
            int offset = seed % 5;
            int group = index / 5;
            int bonus = index / 10;
            int multiplier = seed / 5;
            offset = index + offset + multiplier * bonus;

            int cipherIndex = group * 5 + offset % 5;
            if (cipherIndex < cipher.Length)
            {
              output.Append(cipher[cipherIndex]);
            }
          }
          else
          {
            output.Append(c);
          }

          interpretation.Append(c);
        }
        else
        {
          interpretation.Append(cipher[Random.Shared.Next(0, cipher.Length)]);
          output.Append(cipher[Random.Shared.Next(0, cipher.Length)]);
        }
      }

      return new LanguageOutput(language, interpretation.ToString(), output.ToString());
    }

    private static int GetFluencyFromProficiency(int proficiency)
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
  }
}
