﻿using System;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using squittal.ScrimPlanetmans.ScrimMatch.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace squittal.ScrimPlanetmans.Logging
{
    public class RulesetFileHandler
    {
        public async static Task<bool> WriteToJsonFile(string fileName, JsonRuleset ruleset)
        {
            var confPath = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()["AppSettings:RuleSetsDirName"];
            var basePath = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var rulesetsDirectory = confPath ?? Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "..","rulesets"));

            var path = fileName.EndsWith(".json") ? Path.Combine(rulesetsDirectory,fileName) : Path.Combine(rulesetsDirectory,$"{fileName}.json");

            try
            {
                using (FileStream fileStream = File.Create(path))
                {
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true
                    };

                    await JsonSerializer.SerializeAsync(fileStream, ruleset, options);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async static Task<JsonRuleset> ReadFromJsonFile(string fileName)
        {
            var confPath = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()["AppSettings:RuleSetsDirName"];
            var basePath = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var rulesetsDirectory = confPath ?? Path.GetFullPath(Path.Combine(basePath, "..", "..", "..","..", "rulesets"));
            var path = fileName.EndsWith(".json") ? Path.Combine(rulesetsDirectory, fileName) : Path.Combine(rulesetsDirectory, $"{fileName}.json");

            try
            {
                using (FileStream fileStream = File.OpenRead(path))
                {
                    var ruleset = await JsonSerializer.DeserializeAsync<JsonRuleset>(fileStream);

                    return ruleset;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");
                
                return null;
            }
        }

        public static IEnumerable<string> GetJsonRulesetFileNames()
        {
            var confPath = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()["AppSettings:RuleSetsDirName"];
            var basePath = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var rulesetsDirectory = confPath ?? Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "..", "rulesets"));
     
            var rulesets = new List<string>();

            try
            {
                var files = Directory.GetFiles(rulesetsDirectory);

                foreach (var file in files)
                {
                    if (!file.EndsWith(".json"))
                    {
                        continue;
                    }

                    rulesets.Add(Path.GetFileName(file));
                }

                return rulesets.OrderBy(f => f).ToList();
            }
            catch
            {
                // Ignore
                return null;
            }
        }
    }
}
