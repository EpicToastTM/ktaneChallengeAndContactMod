using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Linq;
using KModkit;

public class moduleScript : MonoBehaviour {

    public KMAudio Audio;
    public KMBombModule Module;
    public KMBombInfo Info;
    public KMRuleSeedable ruleseed;
    public KMSelectable contact, enter;
    public KMSelectable[] keys;
    public TextMesh[] keyText;
    public TextMesh clue, letters, challenge, timer;
    public GameObject ActualModule;
    public KMSelectable[] trumpets;
    public string TwitchHelpMessage = "Use !{0} submit [word] to submit a word. Words containing anything other than letters are ignored.";

    private static int _moduleIDCounter = 1;
    private int _moduleID = 0;

    private bool solved = false, lightsOn = false;

    private string[] clues = { "Are you a solvable\nvanilla module?", "Are you a module by\nRoyal_Flu$h?", "Are you a module by\nTimwi?", "Are you the other word\nin a two-word maze\nmodule?",
                                   "Are you the last word\nof a module that\nrequires audio?", "Are you the other word\nof a module with the\nword 'Square' in it?",
                                   "Are you a needy\nmodule?", "Are you a modded port\nin KTaNE?","Are you a vanilla\nindicator label?", "Are you a modded\nmodule with rule-seed\nsupport?",
                                   "Are you the other word\nof a module with the\nword 'Button' or\n'Buttons' in it?", "Are you the other word\nof a module with the\nword 'Wire' or 'Wires'\nin it?",
                                   "Are you a one-word\nsolvable module\nwithout the letters\n'E' or 'A'?", "Are you the last word\nof a music-related\nmodule?",
                                   "Are you a person\nin the module 'Ice\nCream?'", "Are you one of the\nareas in the module\n'Murder?'", "Contact.", "Are you an item from\nthe module\nAdventure Game?",
                                   "Are you a module that\nshould NOT be solved\nfor Turn the Keys?", "Are you a possible\nword from Anagrams?", "Are you a disease from\nDr. Doctor?",
                                   "Are you a\nMonsplode™?" };

    // There are 22 questions.

    private string[] question1Answers = { "keypad", "maze", "memory", "password", "wires" };
    private string[] question2Answers = { "accumulation", "algebra", "blockbusters", "catchphrase", "coffeebucks", "countdown", "hieroglyphics", "homophones", "lightspeed", "maintenance", "modulo", "poker",
                                          "quintuples", "retirement", "skyrim", "snooker", "westeros" };
    private string[] question3Answers = { "battleship", "bitmaps", "braille", "coordinates", "friendship", "gridlock", "hexamaze", "hogwarts", "kudosudoku", "lasers", "mafia", "mahjong",
                                          "souvenir", "superlogic", "tennis", "yahtzee", "zoo" };
    private string[] question4Answers = { "blind", "boolean", "factory", "module", "polyhedral", "scrambler", "usa" };
    private string[] question5Answers = { "code", "coffeebucks", "listening", "safe", "samples", "sequence", "sequencer", "sounds", "kudosudoku" };
    private string[] question6Answers = { "colored", "decolored", "discolored", "divided", "mystic", "uncolored", "button", "varicolored" };
    private string[] question7Answers = { "aa", "alpha", "determinants", "draw", "edgework", "filibuster", "knob", "math", "tetris", "wingdings" };
    private string[] question8Answers = { "ac", "hdmi", "pcmcia", "usb", "vga" };
    private string[] question9Answers = { "bob", "car", "clr", "frk", "frq", "ind", "msa", "nsa", "sig", "snd", "trn" };
    private string[] question10Answers = { "bitmaps", "boggle", "fizzbuzz", "friendship", "mahjong", "radiator" };
    private string[] question11Answers = { "broken", "complicated", "logical", "masher", "rapid", "sequence", "spinning", "square", "the" };
    private string[] question12Answers = { "complicated", "perplexing", "placement", "sequence", "seven", "skinny", "spaghetti", "the" };
    private string[] question13Answers = { "cooking", "countdown", "fizzbuzz", "functions", "gridlock", "hunting", "instructions", "kudosudoku", "logic", "modulo", "plumbing", "rhythms", "scripting", "sink", "skyrim", "synonyms", "zoni", "zoo" };
    private string[] question14Answers = { "chords", "jukebox", "keys", "qualities", "rhythms", "samples", "sings", "sequence", "sequencer" };
    private string[] question15Answers = { "adam", "ashley", "bob", "cheryl", "dave", "gary", "george", "jacob", "jade", "jessica", "mike", "pat", "sally", "sam", "sean", "simon", "taylor", "tim", "tom", "victor" };
    private string[] question16Answers = { "ballroom", "conservatory", "hall", "kitchen", "library", "lounge", "study" };
    private string[] question17Answers = { "alfa", "bravo", "charlie", "delta", "echo", "foxtrot", "golf", "hotel", "india", "juliett", "kilo", "lima", "mike", "november", "oscar", "papa", "quebec", "romeo", "sierra", "tango", "uniform", "victor", "whiskey", "yankee", "zulu" };
    private string[] question18Answers = { "balloon", "battery", "bellows", "feather", "lamp", "moonstone", "potion", "stepladder", "sunstone", "symbol", "ticket", "trophy" };
    private string[] question19Answers = { "astrology", "cryptography", "maze", "memory", "plumbing", "semaphore", "switches" };
    private string[] question20Answers = { "barely", "bleary", "caller", "cellar", "duster", "looped", "master", "poodle", "rashes", "recall", "rescue", "rudest", "rusted", "seated", "secure", "sedate",
                                           "shares", "shears", "stream", "tamers", "teased" };
    private string[] question21Answers = { "braintenance", "detonession", "emojilepsy", "hrv", "indicitis", "jaundry", "jukepox", "legomania", "neurolysis", "ocd", "orientitis",
                                           "quackgrounds", "tetrinus", "verticode", "widgeting", "xmas", "zooties" };
    private string[] question22Answers = { "aluga", "asteran", "bob", "buhar", "caadarim", "clondar", "docsplode", "flaurim", "gloorim", "lanaluff", "lugirit", "magmy", "melbor", "mountoise", "myrchat", "nibs",
                                           "percy", "pouse", "ukkens", "vellarim", "violan", "zapra", "zenlad" };

    private string[] answers = { "answer0", "answer1", "answer2" };

    private bool[] containChecker = { true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false };

    private string[] possibleFinalAnswers = { "keypad", "keys", "blockbusters", "blind", "coffeebucks", "countdown", "coordinates", "code", "colored", "complicated", "cooking",
                                          "conservatory", "catchphrase", "car", "snooker", "snd", "lightspeed", "listening", "library", "maintenance", "mafia", "mahjong", "maze", "math", "masher",
                                          "poker", "polyhedral", "tennis", "tetris", "braille", "broken", "battleship", "ballroom", "friendship", "frq", "frk", "boolean", "bob", "boggle",
                                          "usa", "usb", "safe", "samples", "sally", "sam", "filibuster", "sig", "simon", "sink", "sings", "radiator",
                                          "rapid", "logical", "logic", "lounge", "sequence", "sean", "placement", "plumbing", "chords", "cheryl", "jade", "password",
                                          "pat", "ind", "instructions", "india", "quebec", "qualities", "quintuples", "balloon", "battery", "papa", "potion", "ashley", "accumulation", "algebra",
                                          "semaphore", "determinants", "delta", "looped", "master", "poodle", "rashes", "recall", "rescue", "rusted", "teased", "braintenance", "bravo",
                                          "detonession", "indicitis", "juliett", "jaundry", "jukepox", "jukepox", "quackgrounds", "tetrinus", "widgeting", "wires", "zoo", "zooties",
                                          "caller", "astrology", "jacob", "ticket", "tim", "retirement", "seated", "secure", "sedate", "stream", "stepladder", "study", "taylor", "tamers", "aluga",
                                          "asteran", "caadarim", "button", "buhar", "clr", "clondar", "lasers", "lanaluff", "lamp", "magmy", "memory", "melbor", "moonstone", "mountoise", "mystic", "myrchat",
                                          "percy", "perplexing", "pouse", "verticode", "vellarim", "spinning", "spaghetti", "hotel", "hogwarts", "zoni", "decolored", "homophones", "module",
                                          "wingdings", "modulo", "draw", "aa", "alpha", "seven" };
    // 145 possible final answers.
    
    private string[] possibleSecondAnswers = { "keypad", "keys", "blockbusters", "blind", "coordinates", "code", "colored", "complicated", "cooking",
                                          "conservatory", "catchphrase", "car", "snooker", "snd", "lightspeed", "listening", "library", "maintenance", "mafia", "maze", "math", "masher",
                                          "poker", "polyhedral", "tennis", "tetris", "braille", "broken", "battleship", "ballroom", "friendship", "frk", "boolean", "bob", "boggle",
                                          "usa", "usb", "safe", "sally", "fizzbuzz", "sig", "simon", "sink", "sings", "radiator",
                                          "rapid", "logical", "logic", "lounge", "sequence", "sean", "placement", "plumbing", "chords", "cheryl", "jacob", "password",
                                          "pat", "ind", "instructions", "india", "quebec", "qualities", "quintuples", "balloon", "papa", "potion", "astrology", "ac", "semaphore",
                                          "determinants", "delta", "looped", "master", "poodle", "rashes", "recall", "rudest", "teased", "braintenance", "bravo", "detonession",
                                          "indicitis", "jaundry", "jukebox", "jukepox", "quackgrounds", "tetrinus", "widgeting", "wires", "zooties", "algebra", "caller",
                                          "ashley", "ticket", "tim", "retirement", "seated", "stream", "stepladder", "study", "taylor", "tamers", "aluga", "asteran", "caadarim", "button", "buhar", "clr",
                                          "clondar", "lamp", "lanaluff", "lasers", "magmy", "memory", "melbor", "moonstone", "mountoise", "myrchat", "mystic", "perplexing", "percy", "pouse", "verticode",
                                          "vellarim", "spinning", "spaghetti", "hotel", "hogwarts", "zoni", "decolored", "homophones", "module", "wingdings", "modulo", "draw", "aa", "alpha" };

    // 133 possible second answers.

    private string[] possibleFirstAnswers = { "coordinates", "code", "colored", "complicated", "cooking",
                                          "conservatory", "lightspeed", "listening", "library", "maintenance", "mafia", "math", "masher", "frk", "bob", "sig", "sink", "logical", "logic",
                                          "knob", "pcmcia", "msa", "trn", "the", "rhythms", "kitchen", "study", "ind", "instructions", "india",
                                          "quebec", "qualities", "quintuples", "kudosudoku", "balloon", "battleship", "ballroom",
                                          "papa", "potion", "stepladder", "adam", "semaphore", "barely", "caller", "duster", "dave",
                                          "looped", "master", "poodle", "poker", "polyhedral", "rashes", "rapid", "radiator", "tennis", "tetris", "braintenance", "broken", "bravo",
                                          "detonession", "determinants", "delta", "indicitis", "jaundry", "jacob", "juliett", "jukebox", "legomania", "quackgrounds", "tetrinus", "whiskey", "zulu", "car",
                                          "catchphrase", "seated", "aluga", "astrology", "caadarim", "docsplode", "flaurim", "lanaluff", "lamp", "magmy", "ukkens",
                                          "uncolored", "vga", "victor", "zapra", "moonstone", "scripting", "factory", "hexamaze", "hunting", "hdmi", "hrv", "hall", "mike",
                                          "varicolored", "zoni", "zooties", "cryptography", "decolored", "hotel", "hogwarts", "module", "wingdings", "wires", "widgeting", "modulo", "aa" };

    // 96 possible first answers.

    private string[] alphabet = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
    private int alphabetPos = 0;
    private string[] keyLetters = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
    private string word = "";
    
    private float seconds = 0;
    private bool flipped = false;

    private int counter = 0;
    private string[] slightlyMorePossibleAnswers = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
    private int stageNum = 0;
    private int otherCounter = 0;

    private bool wasSolutionCorrect = false;

    private string[] firstCipherLetters = { "des", "pac", "ito" };
    private string[] secondCipherLetters = { "o", "o", "f" };
    private string whatUsed = "";

    private string[] displayedLetters = { "", "", "" };
    private int[] letterNumbers = { 1, 1, 1 };

    private string[] usedClues = { "this is", "place holder", "text too" };

    private string[] LeGeNDSounds = { "LeGeNDcontact", "LeGeNDchallenge", "LeGeNDthree", "LeGeNDtwo", "LeGeNDone" };
    private string[] LunaSounds = { "lunaContact", "lunaChallenge", "lunaThree", "lunaTwo", "lunaOne" };
    private string[] TimwiContacts = { "timwiContact1", "timwiContact2", "timwiContact3", "timwiContact4", "timwiContact5", "timwiContact6", "timwiContact7" };
    private string[] TimwiChallenges = { "timwiChallenge1", "timwiChallenge2", "timwiChallenge3" };
    private string[] TimwiCounts = { "timwiThree1", "timwiThree2", "timwiThree3", "timwiTwo1", "timwiTwo2", "timwiTwo3", "timwiOne1", "timwiOne2", "timwiOne3" };
    private string[] YabbaSounds = { "yabbaContact", "yabbaChallenge", "yabbaThree", "yabbaTwo", "yabbaOne" };
    private string[] weterSounds = { "weterContact", "weterChallenge", "weterThree", "weterTwo", "weterOne" };
    private string[] jerrySounds = { "JerryErisContact", "JerryErisChallenge", "JerryEris3", "JerryEris2", "JerryEris1" };

    private int randomChallenge = 0;
    private int randomSound = 0;

    private string commandEnd;
    private bool soundPlaying = false;

    private float[] rotationAmount = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private int[] firstRule = { 6, 0 }; // Holds the rule for which cipher is used. First # is the number used, second # is the condition it must be.
    // First #: 0-5: sum of #'th digit of all two factors. 6: solved modules. 7: unsolved modules. 8: # of strikes.
    // Second #: EVEN, ODD, PRIME, PRIMEN'T, DIVISIBLE BY 3, NOT DIVISIBLE BY 3.
    private int[] unicornRule = { 0, 0 }; // Holds the unicorn rule. (Unicorn rule will always involve the lit BOB indicator.)
    // First #: 0: batteries, 1: battery holders, 2: indicators, 3: ports, 4: port plates, 5: two factors. The second number is what it must be equal to.
    private int firstStageRule = 6; // Basically just unicornRule[0], but with Info.GetModuleNames.Count().
    private string[] cipherNames = { "rot13", "atbash" }; 

    private readonly int[] primes = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103,
    107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199, 211, 223, 227, 281, 283, 293, 307, 311, 313,
    317, 331, 337, 347, 349, 353, 359, 367, 373, 379, 383, 389, 397, 401, 409, 419, 421, 431, 433, 439, 443, 449, 457, 461, 463, 467, 479, 487,
    491, 499, 503, 509, 521, 523, 541, 547, 557, 563, 569, 571, 577, 587, 593, 599, 601, 607, 613, 617, 619, 631, 641, 643, 647, 653, 659, 661,
    673, 677, 683, 691, 701, 709, 719, 727, 733, 739, 743, 751, 757, 761, 769, 773, 787, 797, 809, 811, 821, 823, 827, 829, 839, 853, 857, 859,
    863, 877, 881, 883, 887, 907, 911, 919, 929, 937, 941, 947, 953, 967, 971, 977, 983, 991, 997 };

    int[] ruleAmounts = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    void Start() {
        _moduleID = _moduleIDCounter++;
        Module.OnActivate += Activate;
    }

    void Activate() {
        lightsOn = true;

        letters.text = "";

        Init();
    }

    void Init()
    {
        counter = 0;
        otherCounter = 0;
        randomChallenge = Random.Range(0, 7);

        for (int i = 0; i < 43; i++)
        {
            slightlyMorePossibleAnswers[i] = "";
        }

        for (int i = 0; i < 26; i++)
        {
            while (rotationAmount[i] != 0)
            {
                keyText[i].transform.Rotate(Vector3.forward, 1);
                rotationAmount[i]--;
            }
        }

        contact.OnInteract += delegate ()
        {
            contact.AddInteractionPunch();
            if (lightsOn)
            {
                if (!flipped)
                {
                    if (!solved)
                    {
                        Contact();

                        playSounds(0);

                        if (!flipped)
                        {
                            flipped = true;
                        }

                        else
                        {
                            flipped = false;
                        }
                    }
                }
            }
            return false;
        };

        enter.OnInteract += delegate ()
        {
            enter.AddInteractionPunch();
            if (flipped)
            {
                Challenge();

                if (!flipped)
                {
                    flipped = true;
                }

                else
                {
                    flipped = false;
                }
            }

            return false;
        };

        for (int i = 0; i < 26; i++)
        {
            keys[i].OnInteract = KeyPressed(i);
        }

        trumpets[0].OnInteract += delegate ()
        {
            Audio.PlaySoundAtTransform("DootDoot", ActualModule.transform);
            return false;
        };

        trumpets[1].OnInteract += delegate ()
        {
            Audio.PlaySoundAtTransform("DootDoot", ActualModule.transform);
            return false;
        };

        // Start of module generation
        
        answers[0] = possibleFirstAnswers[Random.Range(0, possibleFirstAnswers.Length)];
        
        for (int i = 0; i < possibleSecondAnswers.Length; i++)
        {
            if (possibleSecondAnswers[i].Substring(0, 1) == answers[0].Substring(0, 1) && answers[0] != possibleSecondAnswers[i])
            {
                slightlyMorePossibleAnswers[counter] = possibleSecondAnswers[i];
                counter++;
            }
        }

        answers[1] = slightlyMorePossibleAnswers[Random.Range(0, counter)];

        for (int i = 0; i < counter; i++)
        {
            slightlyMorePossibleAnswers[i] = "";
        }

        counter = 0;
        
        for (int i = 0; i < possibleFinalAnswers.Length; i++)
        {
            if (possibleFinalAnswers[i].Substring(0, 2) == answers[1].Substring(0, 2) && answers[0] != possibleFinalAnswers[i] && answers[1] != possibleFinalAnswers[i])
            {
                slightlyMorePossibleAnswers[counter] = possibleFinalAnswers[i];
                counter++;
            }
        }

        answers[2] = slightlyMorePossibleAnswers[Random.Range(0, counter)];

        for (int x = 0; x < 3; x++)
        {
            for (int i = 0; i < counter; i++)
            {
                slightlyMorePossibleAnswers[i] = "";
            }

            CheckIfContains(x);
            counter = 0;

            for (int i = 0; i < 22; i++)
            {
                if (containChecker[i])
                {
                    slightlyMorePossibleAnswers[counter] = clues[i];
                    counter++;
                }
            }

            otherCounter = Random.Range(0, counter);
            usedClues[x] = slightlyMorePossibleAnswers[otherCounter];
        }

        Debug.LogFormat("[Challenge & Contact #{0}] The words are {1}, {2}, and {3}.", _moduleID, answers[0], answers[1], answers[2]);

        Debug.LogFormat("[Challenge & Contact #{0}] The clues are {1}, {2}, and {3}.", _moduleID, usedClues[0].Replace("\n", " "), usedClues[1].Replace("\n", " "), usedClues[2].Replace("\n", " "));

        // End of module generation
        
        for (int i = 0; i < 3; i++)
        {
            displayedLetters[i] = "";
        }

        for (int i = 0; i < 3; i++)
        {
            for (int x = 0; x < 26; x++)
            {
                if (alphabet[x] == answers[2].Substring(i, 1))
                {
                    letterNumbers[i] = x;
                    break;
                }
            }

            firstCipherLetters[i] = alphabet[(letterNumbers[i] + 13) % 26];
            secondCipherLetters[i] = alphabet[25 - letterNumbers[i]];
        }

        SetUpRuleseed();

        for (int i = 0; i < 26; i++)
        {
            keyLetters[i] = "?";
        }

        for (int i = 0; i < 26; i++)
        {
            alphabetPos = Random.Range(0, 26);
            keyLetters[i] = alphabet[alphabetPos];

            for (int x = 0; x < 26; x++)
            {
                if (keyLetters[i] == keyLetters[(i + x) % 26])
                {
                    alphabetPos = (alphabetPos + 1) % 26;
                    keyLetters[i] = alphabet[alphabetPos];

                    x = 0;
                }

                keyText[i].text = keyLetters[i];
            }
        }

        MakeClue();
    }

    void SetUpRuleseed()
    {
        var rnd = ruleseed.GetRNG();
        int[] unicornAmounts = { Info.GetBatteryCount(), Info.GetBatteryHolderCount(), Info.GetIndicators().Count(), Info.GetPortCount(), Info.GetPortPlateCount(), Info.GetTwoFactorCounts(), Info.GetModuleNames().Count() };
        
        if (rnd.Seed != 1)
        {
            int rndNum = rnd.Next(5, 9);

            if (rndNum == 5)
            {
                firstRule[0] = rnd.Next(0, 6);
            }

            else
            {
                firstRule[0] = rndNum;
            }

            firstRule[1] = rnd.Next(6);

            unicornRule[0] = rnd.Next(6);
            unicornRule[1] = rnd.Next(5);

            firstStageRule = rnd.Next(7);

            Debug.LogFormat("[Challenge & Contact #{0}] This module is using ruleseed, seed #{1}.", _moduleID, rnd.Seed);
        }
        
        if (Info.GetOnIndicators().Contains("BOB") && unicornAmounts[unicornRule[0]] == unicornRule[1])
        {
            for (int i = 0; i < 3; i++)
            {
                displayedLetters[i] = answers[2].Substring(i, 1);
                whatUsed = "nothing";
            }
        }

        else if (firstRule[1] == 0)
        {
            if (unicornAmounts[firstStageRule] % 2 == 0)
            {
                displayedLetters[0] = firstCipherLetters[0];
                whatUsed = cipherNames[0];
            }

            else
            {
                displayedLetters[0] = secondCipherLetters[0];
                whatUsed = cipherNames[1];
            }
        }

        else if (firstRule[1] == 1)
        {
            if (unicornAmounts[firstStageRule] % 2 == 1)
            {
                displayedLetters[0] = firstCipherLetters[0];
                whatUsed = cipherNames[0];
            }

            else
            {
                displayedLetters[0] = secondCipherLetters[0];
                whatUsed = cipherNames[1];
            }
        }

        else if (firstRule[1] == 2)
        {
            if (primes.Contains(unicornAmounts[firstStageRule]))
            {
                displayedLetters[0] = firstCipherLetters[0];
                whatUsed = cipherNames[0];
            }

            else
            {
                displayedLetters[0] = secondCipherLetters[0];
                whatUsed = cipherNames[1];
            }
        }

        else if (firstRule[1] == 3)
        {
            if (!primes.Contains(unicornAmounts[firstStageRule]))
            {
                displayedLetters[0] = firstCipherLetters[0];
                whatUsed = cipherNames[0];
            }

            else
            {
                displayedLetters[0] = secondCipherLetters[0];
                whatUsed = cipherNames[1];
            }
        }

        else if (firstRule[1] == 4)
        {
            if (unicornAmounts[firstStageRule] % 3 == 0)
            {
                displayedLetters[0] = firstCipherLetters[0];
                whatUsed = cipherNames[0];
            }

            else
            {
                displayedLetters[0] = secondCipherLetters[0];
                whatUsed = cipherNames[1];
            }
        }

        else
        {
            if (unicornAmounts[firstStageRule] % 3 != 0)
            {
                displayedLetters[0] = firstCipherLetters[0];
                whatUsed = cipherNames[0];
            }

            else
            {
                displayedLetters[0] = secondCipherLetters[0];
                whatUsed = cipherNames[1];
            }
        }
    }

    void Challenge()
    {
        CheckSolution();

        if (wasSolutionCorrect)
        {
            challenge.color = new Color32(0, 255, 0, 255);

            if (stageNum != 3)
            {
                MakeClue();
            }
        }

        else
        {
            challenge.color = new Color32(255, 0, 0, 255);
            stageNum = 0;
            Init();
        }

        StartCoroutine("Flip");
        StartCoroutine("eraseWord");
        StopCoroutine("Timer");
    }

    void CheckIfContains(int i)
    {
        for (int x = 0; x < 22; x++)
        {
            containChecker[x] = false;
        }

        if (question1Answers.Contains(answers[i]))
        {
            containChecker[0] = true;
        }

        if (question2Answers.Contains(answers[i]))
        {
            containChecker[1] = true;
        }

        if (question3Answers.Contains(answers[i]))
        {
            containChecker[2] = true;
        }

        if (question4Answers.Contains(answers[i]))
        {
            containChecker[3] = true;
        }

        if (question5Answers.Contains(answers[i]))
        {
            containChecker[4] = true;
        }

        if (question6Answers.Contains(answers[i]))
        {
            containChecker[5] = true;
        }

        if (question7Answers.Contains(answers[i]))
        {
            containChecker[6] = true;
        }

        if (question8Answers.Contains(answers[i]))
        {
            containChecker[7] = true;
        }

        if (question9Answers.Contains(answers[i]))
        {
            containChecker[8] = true;
        }

        if (question10Answers.Contains(answers[i]))
        {
            containChecker[9] = true;
        }

        if (question11Answers.Contains(answers[i]))
        {
            containChecker[10] = true;
        }

        if (question12Answers.Contains(answers[i]))
        {
            containChecker[11] = true;
        }

        if (question13Answers.Contains(answers[i]))
        {
            containChecker[12] = true;
        }

        if (question14Answers.Contains(answers[i]))
        {
            containChecker[13] = true;
        }

        if (question15Answers.Contains(answers[i]))
        {
            containChecker[14] = true;
        }

        if (question16Answers.Contains(answers[i]))
        {
            containChecker[15] = true;
        }

        if (question17Answers.Contains(answers[i]))
        {
            containChecker[16] = true;
        }

        if (question18Answers.Contains(answers[i]))
        {
            containChecker[17] = true;
        }

        if (question19Answers.Contains(answers[i]))
        {
            containChecker[18] = true;
        }

        if (question20Answers.Contains(answers[i]))
        {
            containChecker[19] = true;
        }

        if (question21Answers.Contains(answers[i]))
        {
            containChecker[20] = true;
        }

        if (question22Answers.Contains(answers[i]))
        {
            containChecker[21] = true;
        }
    }

    void Contact()
    {
        StartCoroutine("Flip");
        
        challenge.text = "";

        StartCoroutine("Timer");

        if (stageNum != 0)
        {
            for (int i = 0; i < stageNum * Random.Range(0, 3); i++)
            {
                rotationAmount[Random.Range(0,26)] += Random.Range(0, 360);
            }

            for (int i = 0; i < 26; i++)
            {
                if (keyText[i].text == "m" || keyText[i].text == "w")
                {
                    rotationAmount[i] = 0;
                }
                
                keyText[i].transform.Rotate(Vector3.forward, rotationAmount[i]);
            }
        }
    }

    void MakeClue()
    {
        UpdateNumbers();

        if (stageNum > 0)
        {
            letters.text = letters.text + " ";

            if (displayedLetters[stageNum] != "")
            {
                whatUsed = "nothing";
            }

            else if (firstRule[1] == 0)
            {
                if (ruleAmounts[firstRule[0]] % 2 == 0)
                {
                    displayedLetters[stageNum] = firstCipherLetters[stageNum];
                    whatUsed = cipherNames[0];
                }

                else
                {
                    displayedLetters[stageNum] = secondCipherLetters[stageNum];
                    whatUsed = cipherNames[1];
                }
            }

            else if (firstRule[1] == 1)
            {
                if (ruleAmounts[firstRule[0]] % 2 == 1)
                {
                    displayedLetters[stageNum] = firstCipherLetters[stageNum];
                    whatUsed = cipherNames[0];
                }

                else
                {
                    displayedLetters[stageNum] = secondCipherLetters[stageNum];
                    whatUsed = cipherNames[1];
                }
            }

            else if (firstRule[1] == 2)
            {
                if (primes.Contains(ruleAmounts[firstRule[0]]))
                {
                    displayedLetters[stageNum] = firstCipherLetters[stageNum];
                    whatUsed = cipherNames[0];
                }

                else
                {
                    displayedLetters[stageNum] = secondCipherLetters[stageNum];
                    whatUsed = cipherNames[1];
                }
            }

            else if (firstRule[1] == 3)
            {
                if (!primes.Contains(ruleAmounts[firstRule[0]]))
                {
                    displayedLetters[stageNum] = firstCipherLetters[stageNum];
                    whatUsed = cipherNames[0];
                }

                else
                {
                    displayedLetters[stageNum] = secondCipherLetters[stageNum];
                    whatUsed = cipherNames[1];
                }
            }

            else if (firstRule[1] == 4)
            {
                if (ruleAmounts[firstRule[0]] % 3 == 0)
                {
                    displayedLetters[stageNum] = firstCipherLetters[stageNum];
                    whatUsed = cipherNames[0];
                }

                else
                {
                    displayedLetters[stageNum] = secondCipherLetters[stageNum];
                    whatUsed = cipherNames[1];
                }
            }

            else
            {
                if (ruleAmounts[firstRule[0]] % 3 != 0)
                {
                    displayedLetters[stageNum] = firstCipherLetters[stageNum];
                    whatUsed = cipherNames[0];
                }

                else
                {
                    displayedLetters[stageNum] = secondCipherLetters[stageNum];
                    whatUsed = cipherNames[1];
                }
            }
        }

        else
        {
            letters.text = "";
        }

        letters.text = letters.text + displayedLetters[stageNum];

        clue.text = usedClues[stageNum];

        Debug.LogFormat("[Challenge & Contact #{0}] The letters being displayed are {1}{2}{3}.", _moduleID, displayedLetters[0], displayedLetters[1], displayedLetters[2]);
        Debug.LogFormat("[Challenge & Contact #{0}] The cipher used for this stage was {1}.", _moduleID, whatUsed);
    }

    void CheckSolution()
    {
        Debug.LogFormat("[Challenge & Contact #{0}] You submitted {1}. The answer was {2}.", _moduleID, challenge.text, answers[stageNum]);

        if (challenge.text == answers[stageNum])
        {
            if (timer.text != ":[")
            {
                stageNum++;

                if (stageNum == 3)
                {
                    Module.HandlePass();
                    clue.text = "MODULE SOLVED!";
                    letters.text = "G G :)";
                    solved = true;
                }

                wasSolutionCorrect = true;

                timer.text = ":]";

                Debug.LogFormat("[Challenge & Contact #{0}] That was right!", _moduleID);
            }

            else
            {
                wasSolutionCorrect = false;

                Debug.LogFormat("[Challenge & Contact #{0}] You submitted too late.", _moduleID);
            }
        }

        else
        {
            Module.HandleStrike();
            wasSolutionCorrect = false;

            Debug.LogFormat("[Challenge & Contact #{0}] That was the wrong answer.", _moduleID);
        }
    }

    IEnumerator Flip()
    {
        for (int i = 0; i < 20; i++)
        {
            ActualModule.transform.Rotate(Vector3.right, 9);
            yield return new WaitForSeconds(.015f);
        }
    }

    IEnumerator Timer()
    {
        timer.text = "...";

        yield return new WaitForSeconds(1.5f);

        playSounds(1);

        seconds = answers[stageNum].Length / 3.5f * 3f;

        if (seconds < 3)
        {
            seconds = 3;
        }

        Debug.LogFormat("[Challenge & Contact #{0}] You have {1} seconds.", _moduleID, seconds);

        yield return new WaitForSeconds(1);

        for (int i = 0; i < 3; i++)
        {
            while (seconds > (3 - i))
            {
                seconds -= .01f;
                yield return new WaitForSeconds(.01f);

                if (seconds.ToString().Length < 4)
                {
                    timer.text = seconds.ToString();
                }

                else
                {
                    timer.text = seconds.ToString().Substring(0, 4);
                }
            }

            playSounds(2 + i);
        }

        while (seconds > 0)
        {
            seconds -= .01f;
            yield return new WaitForSeconds(.01f);

            if (seconds.ToString().Length < 4)
            {
                timer.text = seconds.ToString();
            }

            else
            {
                timer.text = seconds.ToString().Substring(0, 4);
            }
        }

        timer.text = "GO!";

        yield return new WaitForSeconds(2);

        timer.text = ":[";
    }

    public KMSelectable.OnInteractHandler KeyPressed(int i)
    {
        return delegate
            {
                keys[i].AddInteractionPunch();

                if (flipped)
                {
                    challenge.color = new Color32(255, 255, 255, 255);

                    word = word + keyText[i].text;

                    if (word.Length > 12 || (word.Length > 7 && word.Substring(0, 8) == "Stop it."))
                    {
                        word = "Stop it.";
                        challenge.color = new Color32(255, 0, 0, 255);

                        if (!soundPlaying)
                        {
                            StartCoroutine("WaitForStopIt");
                            StartCoroutine("flashRed");
                        }
                    }

                    challenge.text = word;
                }

                return false;
            };
    }

    IEnumerator eraseWord()
    {
        while (word != "")
        {
            yield return new WaitForSeconds(.01f);

            word = word.Substring(0, word.Length - 1);
            challenge.text = word;
        }
    }

    IEnumerator flashRed()
    {
        for (int i = 0; i < 10; i++)
        {
            if (challenge.color == new Color32(255, 0, 0, 255))
            {
                challenge.color = new Color32(255, 255, 255, 255);
            }

            else
            {
                challenge.color = new Color32(255, 0, 0, 255);
            }

            yield return new WaitForSeconds(.1f);
        }

        challenge.color = new Color32(255, 0, 0, 255);
    }

    IEnumerator ProcessTwitchCommand (string command)
    {
        if (command.ToLowerInvariant().StartsWith("submit "))
        {
            commandEnd = command.Substring(7).ToLowerInvariant();

            for (int i = 0; i < commandEnd.Length; i++)
            {
                if (!alphabet.Contains(commandEnd[i].ToString()))
                {
                    yield break;
                }
            }
            yield return null;
            yield return new KMSelectable[] { contact };

            for (int i = 0; i < commandEnd.Length; i++)
            {
                for (int x = 0; x < 26; x++)
                {
                    if (keyText[x].text == commandEnd[i].ToString())
                    {
                        yield return null;
                        yield return new KMSelectable[] { keys[x] };
                    }
                }
            }
            yield return new KMSelectable[] { enter };
        }

        else
        {
            yield break;
        }
    }

    IEnumerator WaitForStopIt()
    {
        Audio.PlaySoundAtTransform("StopIt", ActualModule.transform);
        soundPlaying = true;
        if (Random.Range(0, 10) == 0)
        {
            StartCoroutine("FullFlip");
        }
        yield return new WaitForSeconds(1.9f);
        soundPlaying = false;
    }

    IEnumerator FullFlip()
    {
        for (int i = 0; i < 72; i++)
        {
            ActualModule.transform.Rotate(Vector3.right, 30);
            yield return new WaitForSeconds(.0001f);
        }
    }

    private void playSounds(int soundNum)
    {
        if (soundNum == 0)
        {
            randomSound = Random.Range(0, 6);
            if (randomSound == randomChallenge)
            {
                randomSound = (randomSound + 1) % 6;
            }

            if (randomSound == 0)
            {
                Audio.PlaySoundAtTransform(LeGeNDSounds[0], ActualModule.transform);
            }

            else if (randomSound == 1)
            {
                Audio.PlaySoundAtTransform(LunaSounds[0], ActualModule.transform);
            }

            else if (randomSound == 2)
            {
                Audio.PlaySoundAtTransform(TimwiContacts[Random.Range(0, 7)], ActualModule.transform);
            }

            else if (randomSound == 3)
            {
                Audio.PlaySoundAtTransform(YabbaSounds[0], ActualModule.transform);
            }

            else if (randomSound == 4)
            {
                Audio.PlaySoundAtTransform(weterSounds[0], ActualModule.transform);
            }
            
            else if (randomSound == 5)
            {
                Audio.PlaySoundAtTransform(jerrySounds[0], ActualModule.transform);
            }
        }

        else
        {
            if (randomChallenge == 0)
            {
                Audio.PlaySoundAtTransform(LeGeNDSounds[soundNum], ActualModule.transform);
            }

            else if (randomChallenge == 1)
            {
                Audio.PlaySoundAtTransform(LunaSounds[soundNum], ActualModule.transform);
            }

            else if (randomChallenge == 2)
            {
                if (soundNum == 1)
                {
                    Audio.PlaySoundAtTransform(TimwiChallenges[Random.Range(0,3)], ActualModule.transform);
                }
                
                else
                {
                    Audio.PlaySoundAtTransform(TimwiCounts[(soundNum - 2) * 3 + Random.Range(0,3)], ActualModule.transform);
                }
            }

            else if (randomChallenge == 3)
            {
                Audio.PlaySoundAtTransform(YabbaSounds[soundNum], ActualModule.transform);
            }
            
            else if (randomChallenge == 4)
            {
                Audio.PlaySoundAtTransform(weterSounds[soundNum], ActualModule.transform);
            }

            else if (randomChallenge == 5)
            {
                Audio.PlaySoundAtTransform(jerrySounds[soundNum], ActualModule.transform);
            }
        }
    }

    void UpdateNumbers()
    {
        for (int i = 0; i < 6; i++)
        {
            ruleAmounts[i] = 0;
            foreach (var twoFactor in Info.GetTwoFactorCodes())
            {
                if (i == 0)
                {
                    ruleAmounts[i] += twoFactor % 10;
                }

                else if (i == 1)
                {
                    ruleAmounts[i] += twoFactor / 10 % 10;
                }

                else if (i == 2)
                {
                    ruleAmounts[i] += twoFactor / 100 % 10;
                }

                else if (i == 3)
                {
                    ruleAmounts[i] += twoFactor / 1000 % 10;
                }

                else if (i == 4)
                {
                    ruleAmounts[i] += twoFactor / 10000 % 10;
                }

                else
                {
                    ruleAmounts[i] += twoFactor / 100000;
                }
            }
        }

        ruleAmounts[6] = Info.GetSolvedModuleNames().Count;
        ruleAmounts[7] = Info.GetSolvableModuleNames().Count - Info.GetSolvedModuleNames().Count;
        ruleAmounts[8] = Info.GetStrikes();
    }
}