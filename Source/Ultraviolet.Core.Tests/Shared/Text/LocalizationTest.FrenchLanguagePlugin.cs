using System;
using System.Collections.Generic;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Core.Tests.Text
{
    partial class LocalizationTest
    {
        private class FrenchLocalizationPlugin : ILocalizationPlugin
        {
            public String Culture => null;
            public String Language => "fr";

            public IEnumerable<LocalizationMatchEvaluatorData> GetMatchEvaluators()
            {
                return new LocalizationMatchEvaluatorData[]
                {
                    new LocalizationMatchEvaluatorData("Def_Art", new LocalizationMatchEvaluator((src, target) =>
                    {
                        if (target.HasProperty("pluriel")) return "Les ";
                        if (target.HasProperty("voyelle")) return "L'";
                        if (target.HasProperty("masculin")) return "Le ";
                        if (target.HasProperty("féminin")) return "La ";
                        return String.Empty;
                    })),
                    new LocalizationMatchEvaluatorData("def_art", new LocalizationMatchEvaluator((src, target) =>
                    {
                        if (target.HasProperty("pluriel")) return "les ";
                        if (target.HasProperty("voyelle")) return "l'";
                        if (target.HasProperty("masculin")) return "le ";
                        if (target.HasProperty("féminin")) return "la ";
                        return String.Empty;
                    })),
                    new LocalizationMatchEvaluatorData("adj", new LocalizationMatchEvaluator((src, target) =>
                    {
                        if (target.HasProperty("pluriel"))
                        {
                            return target.HasProperty("féminin") ?
                                src.GetVariant("plur_féminin") :
                                src.GetVariant("plur_masculin");
                        }
                        else
                        {
                            return target.HasProperty("féminin") ?
                                src.GetVariant("sing_féminin") :
                                src.GetVariant("sing_masculin");
                        }
                    })),
                };
            }

            public IEnumerable<LocalizationPluralityEvaluator> GetPluralityEvaluators()
            {
                return new[]
                {
                    new LocalizationPluralityEvaluator((src, qty) => qty == 1 ? "singulier" : "pluriel")
                };
            }
        }
    }
}