using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Hangman
{
    public partial class MainWindow : Window
    {
        string answer;
        string playerguess;
        enum Difficulty {Easy, Hard}
        Difficulty difficulty = Difficulty.Easy;
        int wrongguesses = 0;
        int playerwins;
        int playerloses;

        string[] easywords = {
            "account", "act", "addition", "adjustment", "advertisement", "agreement", "air", "amount", "amusement", "animal",
            "answer", "apparatus", "approval", "argument", "art", "attack", "attempt", "attention", "attraction", "authority",
            "back", "balance", "base", "behaviour", "belief", "birth", "bit", "bite", "blood", "blow",
            "body", "brass", "bread", "breath", "brother", "building", "burn", "burst", "business", "butter",
            "canvas", "care", "cause", "chalk", "chance", "change", "cloth", "coal", "colour", "comfort",
            "committee", "company", "comparison", "competition", "condition", "connection", "control", "cook", "copper", "copy",
            "cork", "cotton", "cough", "country", "cover", "crack", "credit", "crime", "crush", "cry",
            "current", "curve", "damage", "danger", "daughter", "day", "death", "debt", "decision", "degree",
            "design", "desire", "destruction", "detail", "development", "digestion", "direction", "discovery", "discussion", "disease",
            "disgust", "distance", "distribution", "division", "doubt", "drink", "driving", "dust", "earth", "edge", //100
            "education", "effect", "end", "error", "event", "example", "exchange", "existence", "expansion", "experience",
            "expert", "fact", "fall", "family", "father", "fear", "feeling", "fiction", "field", "fight",
            "fire", "flame", "flight", "flower", "fold", "food", "force", "form", "friend", "front",
            "fruit", "glass", "gold", "government", "grain", "grass", "grip", "group", "growth", "guide",
            "harbour", "harmony", "hate", "hearing", "heat", "help", "history", "hole", "hope", "hour",
            "humour", "ice", "idea", "impulse", "increase", "industry", "ink", "insect", "instrument", "insurance",
            "interest", "invention", "iron", "jelly", "join", "journey", "judge", "jump", "kick", "kiss",
            "knowledge", "land", "language", "laugh", "law", "lead", "learning", "leather", "letter", "level",
            "lift", "light", "limit", "linen", "liquid", "list", "look", "loss", "love", "machine",
            "man", "manager", "mark", "market", "mass", "meal", "measure", "meat", "meeting", "memory", //200
            "metal", "middle", "milk", "mind", "mine", "minute", "mist", "money", "month", "morning",
            "mother", "motion", "mountain", "move", "music", "name", "nation", "need", "news", "night",
            "noise", "note", "number", "observation", "offer", "oil", "operation", "opinion", "order", "organization",
            "ornament", "owner", "page", "pain", "paint", "paper", "part", "paste", "payment", "peace",
            "person", "place", "plant", "play", "pleasure", "point", "poison", "polish", "porter", "position",
            "powder", "power", "price", "print", "process", "produce", "profit", "property", "prose", "protest",
            "pull", "punishment", "purpose", "push", "quality", "question", "rain", "range", "rate", "ray",
            "reaction", "reading", "reason", "record", "regret", "relation", "religion", "representative", "request", "respect",
            "rest", "reward", "rhythm", "rice", "river", "road", "roll", "room", "rub", "rule",
            "run", "salt", "sand", "scale", "science", "sea", "seat", "secretary", "selection", "self", //300
            "sense", "servant", "sex", "shade", "shake", "shame", "shock", "side", "sign", "silk",
            "silver", "sister", "size", "sky", "sleep", "slip", "slope", "smash", "smell", "smile",
            "smoke", "sneeze", "snow", "soap", "society", "son", "song", "sort", "sound", "soup",
            "space", "stage", "start", "statement", "steam", "steel", "step", "stitch", "stone", "stop",
            "story", "stretch", "structure", "substance", "sugar", "suggestion", "summer", "support", "surprise", "swim",
            "system", "talk", "taste", "tax", "teaching", "tendency", "test", "theory", "thing", "thought",
            "thunder", "time", "tin", "top", "touch", "trade", "transport", "trick", "trouble", "turn",
            "twist", "unit", "use", "value", "verse", "vessel", "view", "voice", "walk", "war",
            "wash", "waste", "water", "wave", "wax", "way", "weather", "week", "weight","wind",
            "wine", "winter", "woman", "wood", "wool", "word", "work", "wound", "writing", "year", //400
            "angle", "ant", "apple", "arch", "arm", "army", "baby", "bag", "ball", "band",
            "basin", "basket", "bath", "bed", "bee", "bell", "berry", "bird", "blade", "board",
            "boat", "bone", "book", "boot", "bottle", "box", "boy", "brain", "brake", "branch",
            "brick", "bridge", "brush", "bucket", "bulb", "button", "cake", "camera", "card", "cart",
            "carriage", "cat", "chain", "cheese", "chest", "chin", "church", "circle", "clock", "cloud",
            "coat", "collar", "comb", "cord", "cow", "cup", "curtain", "cushion", "dog", "door",
            "drain", "drawer", "dress", "drop", "ear", "egg", "engine", "eye", "face", "farm",
            "feather", "finger", "fish", "flag", "floor", "fly", "foot", "fork", "fowl", "frame",
            "garden", "girl", "glove", "goat", "gun", "hair", "hammer", "hand", "hat", "head",
            "heart", "hook", "horn", "horse", "hospital", "house", "island", "jewel", "kettle", "key", //500
            "knee", "knife", "knot", "leaf", "leg", "library", "line", "lip", "lock", "map",
            "match", "monkey", "moon", "mouth", "muscle", "nail", "neck", "needle", "nerve", "net",
            "nose", "nut", "office", "orange", "oven", "parcel", "pen", "pencil", "picture", "pig",
            "pin", "pipe", "plane", "plate", "plough", "pocket", "pot", "potato", "prison", "pump",
            "rail", "rat", "receipt", "ring", "rod", "roof", "root", "sail", "school", "scissors",
            "screw", "seed", "sheep", "shelf", "ship", "shirt", "shoe", "skin", "skirt", "snake",
            "sock", "spade", "sponge", "spoon", "spring", "square", "stamp", "star", "station", "stem",
            "stick", "stocking", "stomach", "store", "street", "sun", "table", "tail", "thread", "throat",
            "thumb", "ticket", "toe", "tongue", "tooth", "town", "train", "tray", "tree", "trousers",
            "umbrella", "wall", "watch", "wheel", "whip", "whistle", "window", "wing", "wire", "worm", //600
            "able", "acid", "angry", "automatic", "beautiful", "black", "boiling", "bright", "broken", "brown",
            "cheap", "chemical", "chief", "clean", "clear", "common", "complex", "conscious", "cut", "deep",
            "dependent", "early", "elastic", "electric", "equal", "fat", "fertile", "first", "fixed", "flat",
            "free", "frequent", "full", "general", "good", "great", "grey", "hanging", "happy", "hard",
            "healthy", "high", "hollow", "important", "kind", "like", "living", "long", "married", "material",
            "medical", "military", "natural", "necessary", "new", "normal", "open", "parallel", "past", "physical",
            "political", "poor", "possible", "present", "private", "probable", "quick", "quiet", "ready", "red",
            "regular", "responsible", "right", "round", "same", "second", "separate", "serious", "sharp", "smooth",
            "sticky", "stiff", "straight", "strong", "sudden", "sweet", "tall", "thick", "tight", "tired",
            "true", "violent", "waiting", "warm", "wet", "wide", "wise", "yellow", "young", "awake", //700
            "bad", "bent", "bitter", "blue", "certain", "cold", "complete", "cruel", "dark", "dead",
            "dear", "delicate", "different", "dirty", "dry", "false", "feeble", "female", "foolish", "future",
            "green", "ill", "last", "late", "left", "loose", "loud", "low", "male", "mixed",
            "narrow", "old", "opposite", "public", "rough", "sad", "safe", "secret", "short", "shut",
            "simple", "slow", "small", "soft", "solid", "special", "strange", "thin", "white", "wrong" //750

        };
        string[] hardwords = {
            "awkward", "bagpipes", "banjo", "bungler", "croquet", "crypt", "dwarves", "fervid", "fishhook", "fjord",
            "gazebo", "gypsy", "haiku", "haphazard", "hyphen", "ivory", "jazzy", "jiffy", "jinx", "jukebox",
            "kayak", "kiosk", "klutz", "memento", "mystify", "numbskull", "ostracize", "oxygen", "pajama", "phlegm",
            "pixel", "polka", "quad", "quip", "rhythmic", "rogue", "sphinx", "squawk", "swivel", "toady",
            "twelfth", "unzip", "waxy", "wildebeest", "yacht", "zealous", "zigzag", "zippy", "zombie", "zeitgeist",
            "discombobulated", "anthropomorphisation", "accommodate", "handkerchief", "indict", "cemetery", "conscience", "rhythm", "playwright", "embarrass",
            "millennium", "pharaoh", "liaison", "convalesce", "supersede", "ecstasy", "caribbean", "harass", "aberration", "abjure",
            "abject", "abnegation", "abrogate", "abdicate", "abscond", "abstruse", "accede", "accost", "accretion", "acumen",
            "adamant", "admonish", "adumbrate", "adverse", "advocate", "affluent", "aggrandize", "alacrity", "alias", "ambivalent",
            "amenable", "amorphous", "anachronistic", "anathema", "annex", "antediluvian", "antiseptic", "apathetic", "antithesis", "apocryphal", //100
            "approbation", "arbitrary", "arboreal", "arcane", "archetypal", "arrogate", "ascetic", "aspersion", "assiduous", "atrophy",
            "bane", "bashful", "beguile", "bereft", "blandishment", "bilk", "bombastic", "cajole", "callous", "calumny",
            "camaraderie", "candor", "capitulate", "carouse", "carp", "caucus", "cavort", "circumlocution", "circumscribe", "circumvent",
            "clamor", "cleave", "cobbler", "cogent", "cognizant", "commensurate", "complement", "compunction", "concomitant", "conduit",
            "conflagration", "congruity", "connive", "consign", "constituent", "construe", "contusion", "contrite", "contentious", "contravene",
            "convivial", "corpulence", "covet", "cupidity", "dearth", "debacle", "debauch", "debunk", "defunct", "demagogue",
            "denigrate", "derivative", "despot", "diaphanous", "didactic", "dirge", "disaffected", "discomfit", "disparate", "dispel",
            "disrepute", "divisive", "dogmatic", "dour", "duplicity", "duress", "eclectic", "ebullient", "egregious", "elegy",
            "elicit", "embezzlement", "emend", "emollient", "empirical", "emulate", "enervate", "enfranchise", "engender", "ephemeral",
            "epistolary", "equanimity", "equivocal", "espouse", "evanescent", "evince", "exacerbate", "exhort", "execrable", "exigent", //200
            "expedient", "expiate", "expunge", "extraneous", "extol", "extant", "expurgate", "fallacious", "fatuous", "fetter",
            "flagrant", "foil", "forbearance", "fortuitous", "fractious", "garrulous", "gourmand", "grandiloquent", "gratuitous", "hapless",
            "hegemony", "heterogenous", "iconoclast", "idiosyncratic", "impecunious", "impetuous", "impinge", "impute", "inane", "inchoate",
            "incontrovertible", "incumbent", "inexorable", "inimical", "injunction", "inoculate", "insidious", "instigate", "insurgent", "interlocutor",
            "intimation", "inure", "invective", "intransigent", "inveterate", "irreverence", "knell", "laconic", "largesse", "legerdemain",
            "libertarian", "licentious", "linchpin", "litigant", "maelstrom", "maudlin", "maverick", "mawkish", "maxim", "mendacious",
            "modicum", "morass", "mores", "munificent", "multifarious", "nadir", "negligent", "neophyte", "noisome", "noxious",
            "obdurate", "obfuscate", "obstreperous", "officious", "onerous", "ostensible", "ostracism", "palliate", "panacea", "paradigm",
            "pariah", "partisan", "paucity", "pejorative", "pellucid", "penchant", "penurious", "pert", "pernicious", "pertinacious",
            "phlegmatic", "philanthropic", "pithy", "platitude", "plaudit", "plenitude", "plethora", "portent", "potentate", "preclude", //300
            "predilection", "preponderance", "presage", "probity", "proclivity", "profligate", "promulgate", "proscribe", "protean", "prurient",
            "puerile", "pugnacious", "pulchritude", "punctilious", "quaint", "quixotic", "quandary", "recalcitrant", "redoubtable", "relegate",
            "remiss", "reprieve", "reprobate", "rescind", "requisition", "rife", "sanctimonious", "sanguine", "scurrilous", "semaphore",
            "serendipity", "sobriety", "solicitous", "solipsism", "spurious", "staid", "stolid", "subjugate", "surfeit", "surreptitious",
            "swarthy", "tangential", "tome", "toady", "torpid", "travesty", "trenchant", "trite", "truculent", "turpitude",
            "ubiquitous", "umbrage", "upbraid", "utilitarian", "veracity", "vestige", "vicissitude", "vilify", "virtuoso", "vitriolic",
            "vituperate", "vociferous", "wanton", "winsome", "yoke", "zephyr", "wily", "tirade", "jazz", "eczema",
            "baccalaureate", "counterrevolutionary", "uncharacteristically", "echolocation", "compartmentalization", "counterdemonstration", "crystallographically", "deindustrializations", "departmentalizations", "expressionlessnesses",
            "indistinguishability", "interchangeabilities", "magnetohydrodynamics", "unintelligiblenesses", "atheist", "wren", "perpendicular", "tarnish", "esoteric", "decimation",
            "evisceration", "exsanguination", "annihilation", "vindicated", "quadriplegic", "quadrilateral", "equilateral", "bureaucracy", "normalcy", "semblance", //400
        };

        Random rnd = new Random();
        PopupWindow popup = new PopupWindow();

        float windowwidth = 500;
        float windowheight = 500;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            StartGame(); //Generates a new word on start          
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            SetSizes();
        }

        public void SetSizes()
        {
            SetLabelsSize();
            SetKeysSize();
            SetOtherSize();
        }

        private void SetLabelsSize() //Resizes the sizes and fonts of the objects based on the screens starting size devided by the cuyrrent size. It also sets of the possitioning of the objects using nth term formulas 
        {
            grdLabels.Width = 500 * (this.Width / windowwidth);
            grdLabels.Height = 488 * (this.Height / windowheight);

            for (int i = 0; i < grdLabels.Children.Count; i++)
            {
                ((TextBox)grdLabels.Children[i]).Width = 25 * (this.Width / windowwidth);
                ((TextBox)grdLabels.Children[i]).Height = 25 * (this.Height / windowheight);

                ((TextBox)grdLabels.Children[i]).FontSize = 12 * (this.Height / windowheight);

                var margin = ((TextBox)grdLabels.Children[i]).Margin; //Asigns to a var beacuse you cant asign directly

                if (i < 11)
                {
                    margin.Left = ((28 * i) + 95) * (this.Width / windowwidth); //Places the first 10 labels at the starting number (95) which increments by 28 each time (the distance between the left side of each label) 
                    margin.Top = 246 * (this.Height / windowheight);
                }

                else
                {
                    margin.Left = ((28 * (i - 11)) + 95) * (this.Width / windowwidth); //Places the rest at a lower height and starting at 95 again
                    margin.Top = 274 * (this.Height / windowheight);
                }

                ((TextBox)grdLabels.Children[i]).Margin = margin;
            }
        }

        private void SetKeysSize()
        {
            grdButtons.Width = 492 * (this.Width / windowwidth);
            grdButtons.Height = 469 * (this.Height / windowheight);

            for (int i = 0; i < grdButtons.Children.Count; i++)
            {
                ((Button)grdButtons.Children[i]).Width = 25 * (this.Width / windowwidth);
                ((Button)grdButtons.Children[i]).Height = 25 * (this.Height / windowheight);

                ((Button)grdButtons.Children[i]).FontSize = 12 * (this.Height / windowheight);

                var margin = ((Button)grdButtons.Children[i]).Margin;

                if (i < 10)
                {
                    margin.Left = ((28 * i) + 105) * (this.Width / windowwidth);
                    margin.Top = 339 * (this.Height / windowheight);
                }

                else if (i < 19)
                {
                    margin.Left = ((28 * (i - 10)) + 119) * (this.Width / windowwidth);
                    margin.Top = 367 * (this.Height / windowheight);
                }

                else
                {
                    margin.Left = ((28 * (i - 19)) + 147) * (this.Width / windowwidth);
                    margin.Top = 395 * (this.Height / windowheight);
                }

                ((Button)grdButtons.Children[i]).Margin = margin;
            }
        }

        public void SetOtherSize()
        {
            var margin = txtWinLose.Margin;

            txtWinLose.Width = 143 * (this.Width / windowwidth);
            txtWinLose.Height = 50 * (this.Height / windowheight);
            txtWinLose.FontSize = 36 * (this.Height / windowheight);
            margin.Left = 175 * (this.Width / windowwidth);
            margin.Top = 10 * (this.Height / windowheight);
            txtWinLose.Margin = margin;

            imgPerson.Width = 273 * (this.Width / windowwidth);
            imgPerson.Height = 198 * (this.Height / windowheight);
            margin.Left = 107 * (this.Width / windowwidth);
            margin.Top = 58 * (this.Height / windowheight);
            imgPerson.Margin = margin;

            txtGuess.Width = 224 * (this.Width / windowwidth);
            txtGuess.Height = 25 * (this.Height / windowheight);
            txtGuess.FontSize = 12 * (this.Height / windowheight);
            margin.Left = 107 * (this.Width / windowwidth);
            margin.Top = 315 * (this.Height / windowheight);
            txtGuess.Margin = margin;

            btnGuess.Width = 42 * (this.Width / windowwidth);
            btnGuess.Height = 25 * (this.Height / windowheight);
            btnGuess.FontSize = 12 * (this.Height / windowheight);
            margin.Left = 338 * (this.Width / windowwidth);
            margin.Top = 315 * (this.Height / windowheight);
            btnGuess.Margin = margin;

            btnEasy.Width = 92 * (this.Width / windowwidth);
            btnEasy.Height = 25 * (this.Height / windowheight);
            btnEasy.FontSize = 12 * (this.Height / windowheight);
            margin.Left = 150 * (this.Width / windowwidth);
            margin.Top = 433 * (this.Height / windowheight);
            btnEasy.Margin = margin;

            btnHard.Width = 92 * (this.Width / windowwidth);
            btnHard.Height = 25 * (this.Height / windowheight);
            btnHard.FontSize = 12 * (this.Height / windowheight);
            margin.Left = 247 * (this.Width / windowwidth);
            margin.Top = 433 * (this.Height / windowheight);
            btnHard.Margin = margin;
        }

        private async Task PutTaskDelayanimAsync(int i) //Waits i ms without freezing the program
        {
            await Task.Delay(i);
        }

        public async void StartGame() //Starts a new round 
        {
            SetImage();
            wrongguesses = 0;
            await PutTaskDelayanimAsync(50); //Delay so the code dosent disable one of the the letter keys
            txtGuess.Text = "";
            UpdatePlayerScore();
            ClearLabels();

            if (difficulty == Difficulty.Easy) //Selects a word from the easy words
            {
                answer = easywords[rnd.Next(0, easywords.Length)];
            }

            else if (difficulty == Difficulty.Hard) //Selects a word from the hard words
            {
                answer = hardwords[rnd.Next(0, hardwords.Length)];
            }

            for (int i = 0; i < grdLabels.Children.Count; i++) //Goes though the answer boxes and shows/hises the ones according to the length of the word. ie. If the word is 7 letters long it will show 7 boxes
            {
                if (i <= answer.Length - 1)
                {
                    ((TextBox)grdLabels.Children[i]).Visibility = Visibility.Visible;
                }

                else if (i > answer.Length - 1)
                {
                    ((TextBox)grdLabels.Children[i]).Visibility = Visibility.Hidden;
                }
            }
        }

        public void PlayerGuessLetter(char c) //Whenever the player clicks one of the guess boxes
        {
            for (int i = 0; i < answer.Length; i++) //Goes through the answer and finds which characters are equal to the players guess. ie. if the player guesses q, every q in the answer will be shown
            {
                if (c == (char)answer[i])
                {
                    ((TextBox)grdLabels.Children[i]).Text = c.ToString();
                }
            }

            if (!answer.Contains(c)) //If the leter guessed dose not exsist within the answe thr amount of wrong guessses go up
            {
                wrongguesses++;
            }

            if (wrongguesses > 10) //If the player guesses wrong too many times they lose
            {
                PlayerLose();
            }

            CheckWinLose(); //Checks the answer the player has guessed (via the buttons) 
            SetImage(); //Changes the image to show how many guesses the player has left
        }

        public void CheckWinLose()
        {
            playerguess = "";

            for (int i = 0; i < grdLabels.Children.Count; i++) //Builds a string of the guess from the guessed boxes. ie. if the boxes look like: h_llo world it will return "hllo world"
            {
                if (((TextBox)grdLabels.Children[i]).Text != "")
                {
                    playerguess = playerguess + ((TextBox)grdLabels.Children[i]).Text;
                }
            }

            if (playerguess.ToLower() == answer) //If the built string matches the answer the player wins
            {
                PlayerWin();
            }
        }

        public void SetImage()
        {
            string imagepath;
            imagepath = wrongguesses + ".png";
            imgPerson.Source = new BitmapImage(new Uri(@imagepath, UriKind.Relative));
        }

        private void BtnGuess_Click(object sender, RoutedEventArgs e) //When the player guesses a word by typing it in the box this checks if the word matches
        {
            if (txtGuess.Text.ToLower() == answer.ToLower())
            {
                PlayerWin();
            }

            else
            {
                PlayerLose();
            }
        }

        public void PlayerWin()
        {
            ShowPopup(true);
            playerwins++;
            StartGame();
        }

        public void PlayerLose()
        {
            ShowPopup(false);
            playerloses++;
            StartGame();
        }

        private void ShowPopup(bool win) //Opens a popup window
        {
            popup = new PopupWindow();
            popup.SetVariables(answer);
            popup.SetLabel(win);
            popup.ShowDialog();
        }

        public void UpdatePlayerScore()
        {
            txtWinLose.Text = playerwins + ":" + playerloses;
        }

        public void ClearLabels() //Resets the guess buttons and labels 
        {
            for (int i = 0; i < grdLabels.Children.Count; i++)
            {
                ((TextBox)grdLabels.Children[i]).Text = "";
            }

            for (int i = 0; i < grdButtons.Children.Count; i++)
            {
                ((Button)grdButtons.Children[i]).IsEnabled = true;
            }
        }

        private void BtnEasy_Click(object sender, RoutedEventArgs e) 
        {
            difficulty = Difficulty.Easy;
            DifficultyChange();
        }

        private void BtnHard_Click(object sender, RoutedEventArgs e)
        {
            difficulty = Difficulty.Hard;
            DifficultyChange();
        }

        private void DifficultyChange() //Toggles which difficulty button is enabled and starts a new game
        {
            btnEasy.IsEnabled = !btnEasy.IsEnabled;
            btnHard.IsEnabled = !btnHard.IsEnabled;
            StartGame();
        }

        #region Input for every key

        private void BtnQ_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('q');
            btnQ.IsEnabled = false;
        }

        private void BtnW_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('w');
            btnW.IsEnabled = false;
        }

        private void BtnE_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('e');
            btnE.IsEnabled = false;
        }

        private void BtnR_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('r');
            btnR.IsEnabled = false;
        }

        private void BtnT_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('t');
            btnT.IsEnabled = false;
        }

        private void BtnY_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('y');
            btnY.IsEnabled = false;
        }

        private void BtnU_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('u');
            btnU.IsEnabled = false;
        }

        private void BtnI_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('i');
            btnI.IsEnabled = false;
        }

        private void BtnO_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('o');
            btnO.IsEnabled = false;
        }

        private void BtnP_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('p');
            btnP.IsEnabled = false;
        }

        private void BtnA_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('a');
            btnA.IsEnabled = false;
        }

        private void BtnS_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('s');
            btnS.IsEnabled = false;
        }

        private void BtnD_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('d');
            btnD.IsEnabled = false;
        }

        private void BtnF_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('f');
            btnF.IsEnabled = false;
        }

        private void BtnG_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('g');
            btnG.IsEnabled = false;
        }

        private void BtnH_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('h');
            btnH.IsEnabled = false;
        }

        private void BtnJ_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('j');
            btnJ.IsEnabled = false;
        }

        private void BtnK_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('k');
            btnK.IsEnabled = false;
        }

        private void BtnL_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('l');
            btnL.IsEnabled = false;
        }

        private void BtnZ_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('z');
            btnZ.IsEnabled = false;
        }

        private void BtnX_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('x');
            btnX.IsEnabled = false;
        }

        private void BtnC_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('c');
            btnC.IsEnabled = false;
        }

        private void BtnV_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('v');
            btnV.IsEnabled = false;
        }

        private void BtnB_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('b');
            btnB.IsEnabled = false;
        }

        private void BtnN_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('n');
            btnN.IsEnabled = false;
        }

        private void BtnM_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('m');
            btnM.IsEnabled = false;
        }

        #endregion
    }
}
