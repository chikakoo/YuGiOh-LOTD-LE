let Enums = {
    NameFitlerTypes: {
        WILDCARD: "Wildcard",
        MATCH_WHOLE_WORD: "MatchWholeWord",
        REGEX: "Regex",
        REGEX_ANY_WORD: "RegexAnyWord"
    },

    GeneralCardTypes: {
        MONSTER: "Monster",
        SPELL: "Spell",
        TRAP: "Trap",

        FUSION: "Fusion",
        SYNCHRO: "Synchro",
        XYZ: "Xyz",
        LINK: "Link"
    },
    ExtraGeneralCardTypes: {
        RANDOM_MAIN: "Random Main",
        RANDOM_EXTRA: "Random Extra"
    },

    FilterSetTypes: {
        ROUND_ROBIN: "Round Robin",
        FALLBACK: "Fallback"
    },

    /**
     * Helper to combine the GeneralCardTypes and ExtraGeneralCardTypes
     */
    getAllGeneralCardTypeValues: function() {
        return Object.values(Enums.GeneralCardTypes).concat(Object.values(Enums.ExtraGeneralCardTypes));
    },

    RaceTypes: {
        NORMAL: "Normal",
        CONTINUOUS: "Continuous",

        EQUIP: "Equip",
        QUICK_PLAY: "Quick-Play",
        FIELD: "Field",
        RITUAL: "Ritual",

        COUNTER: "Counter",

        AQUA: "Aqua",
        BEAST: "Beast",
        BEAST_WARRIOR: "Beast-Warrior",
        CYBERSE: "Cyberse",
        DINOSAUR: "Dinosaur",
        DIVINE_BEAST: "Divine-Beast",
        DRAGON: "Dragon",
        FAIRY: "Fairy",
        FIELD: "Fiend",
        FISH: "Fish",
        INSECT: "Insect",
        MACHINE: "Machine",
        PLANT: "Plant",
        PSYCHIC: "Psychic",
        PYRO: "Pyro",
        REPTILE: "Reptile",
        ROCK: "Rock",
        SEA_SERPENT: "Sea Serpent",
        SPELLCASTER: "Spellcaster",
        THUNDER: "Thunder",
        WARRIOR: "Warrior",
        WINGED_BEAST: "Winged Beast",
        WYRM: "Wyrm",
        ZOMBIE: "Zombie"
    },

    Attributes: { //TODO: get the actual list of these from the data
        DARK: "Dark",
        LIGHT: "Light",
        WATER: "Water"
    },

    CardTypes: {
        EFFECT_MONSTER: "Effect Monster",
        FLIP_EFFECT_MONSTER: "Flip Effect Monster",
        FUSION_MONSTER: "Fusion Monster",
        GEMINI_MONSTER: "Gemini Monster",
        LINK_MONSTER: "Link Monster",
        NORMAL_MONSTER: "Normal Monster",
        NORMAL_TUNER_MONSTER: "Normal Tuner Monster",
        PENDULUM_EFFECT_FUSION_MONSTER: "Pendulum Effect Fusion Monster",
        PENDULUM_EFFECT_MONSTER: "Pendulum Effect Monster",
        PENDULUM_FLIP_EFFECT_MONSTER: "Pendulum Flip Effect Monster",
        PENDULUM_NORMAL_MONSTER: "Pendulum Normal Monster",
        PENDULUM_TUNER_EFFECT_MONSTER: "Pendulum Tuner Effect Monster",
        RITUAL_EFFECT_MONSTER: "Ritual Effect Monster",
        RITUAL_MONSTER: "Ritual Monster",
        SPELL_CARD: "Spell Card",
        SPIRIT_MONSTER: "Spirit Monster",
        SYNCHRO_MONSTER: "Synchro Monster",
        SYNCHRO_PENDULUM_EFFECT_MONSTER: "Synchro Pendulum Effect Monster",
        SYNCHRO_TUNER_MONSTER: "Synchro Tuner Monster",
        TOON_MONSTER: "Toon Monster",
        TRAP_CARD: "Trap Card",
        TUNER_MONSTER: "Tuner Monster",
        UNION_EFFECT_MONSTER: "Union Effect Monster",
        XYZ_MONSTER: "XYZ Monster",
        XYZ_PENDULUM_EFFECT_MONSTER: "XYZ Pendulum Effect Monster"
    },

    MonsterTypeFilter: {
        NONE: "None",
        PENDULUM: "Only pendulum cards",
        TUNER: "Only tuner cards",
        NORMAL: "Only normal monster cards"
    }
};