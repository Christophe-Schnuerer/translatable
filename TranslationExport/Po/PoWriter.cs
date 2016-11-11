﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TranslationExport.Po
{
    public class PotWriter
    {
        public void WritePotFile(string filename, Catalog catalog)
        {
            using (var s = File.OpenWrite(filename))
            {
                using (var output = new StreamWriter(s, Encoding.UTF8))
                {
                    output.WriteLine("msgid \"\"");
                    output.WriteLine("msgstr \"\"");
                    output.WriteLine("\"Content-Type: text/plain; charset=UTF-8\"");
                    output.WriteLine();

                    foreach (var poEntry in catalog.Entries)
                    {
                        if (!string.IsNullOrWhiteSpace(poEntry.Comment))
                        {
                            output.WriteLine($"#. {poEntry.Comment}");
                        }

                        if (poEntry.SourceReferences.Any())
                        {
                            var referencesString = string.Join(" ", poEntry.SourceReferences);
                            output.WriteLine($"#: {referencesString}");
                        }

                        if (poEntry is SingularEntry)
                        {
                            var singularEntry = poEntry as SingularEntry;
                            output.WriteLine($"msgid \"{singularEntry.MsgId}\"");
                            output.WriteLine("msgstr \"\"");
                            output.WriteLine();
                        }
                        else if (poEntry is PluralEntry)
                        {
                            var pluralEntry = poEntry as PluralEntry;
                            output.WriteLine($"msgid \"{pluralEntry.MsgIdSingular}\"");
                            output.WriteLine($"msgid_plural \"{pluralEntry.MsgIdPlural}\"");
                            output.WriteLine("msgstr[0] \"\"");
                            output.WriteLine("msgstr[1] \"\"");
                            output.WriteLine();
                        }
                        
                    }
                }
            }
        }
    }
}