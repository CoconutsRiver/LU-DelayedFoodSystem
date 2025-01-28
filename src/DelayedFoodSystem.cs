using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.Reflection;
using Combat;

namespace DelayedFoodSystem


{

    /*
      * DelayedFoodSystem.cs
      * Version: 1.0
      * Author: CoconutsRiver
      * 
      * This BepInEx plugin modifies the game's food and drink system to apply changes over time
      * instead of instantly. The durations for these changes are configurable via a JSON file.
      * 
      * Configuration:
      * - The config.json file should be placed in the BepInEx plugins folder.
      * - It should contain the following structure:
      * {
      *     "foodDuration": 15.0,
      *     "drinkDuration": 10.0
      * }
      * 
      * This file will be created with default values if it does not already exist.
      */

    [BepInPlugin("com.CoconutsRiver.DelayedFoodSystem", "DelayedFoodSystem", "1.0")]
    public class DelayedFoodSystem : BaseUnityPlugin
    {
        private Harmony _harmony;
        public static DelayedFoodSystem Instance { get; private set; }
        private ConfigData _configData;
        private void Awake()
        {
            Logger.LogInfo("DelayedFoodSystem is loaded!");

            _harmony = new Harmony("com.CoconutsRiver.DelayedFoodSystem");
            _harmony.PatchAll();
            Instance = this;

            Logger.LogInfo("Harmony patches applied successfully.");

            LoadConfig();
        }

        private void LoadConfig()
        {
            string configPath = Path.Combine(Paths.PluginPath, "DelayedFoodConfig.json");
            if (File.Exists(configPath))
            {
                string json = File.ReadAllText(configPath);
                _configData = JsonUtility.FromJson<ConfigData>(json);
                Logger.LogInfo("Configuration loaded successfully.");
            }
            else
            {
                _configData = new ConfigData { foodDuration = 15.0f, drinkDuration = 10.0f };
                string json = JsonUtility.ToJson(_configData, true);
                File.WriteAllText(configPath, json);
                Logger.LogInfo("Configuration file created with default values.");
            }
        }

        void OnDestroy()
        {
            _harmony.UnpatchSelf();
            Instance = null;
        }

        public void StartDelayedFoodSystem(float food, float drink)
        {
            StartCoroutine(DelayedFoodRoutine(food, drink, _configData.foodDuration, _configData.drinkDuration));
        }

        private IEnumerator DelayedFoodRoutine(float food, float drink, float foodDuration, float drinkDuration)
        {
            float foodIncrement = food / foodDuration;
            float drinkIncrement = drink / drinkDuration;

            float maxDuration = Mathf.Max(foodDuration, drinkDuration);

            for (float elapsed = 0; elapsed < maxDuration; elapsed += Time.deltaTime)
            {
                if (elapsed < foodDuration)
                {
                    GameCore.instance.foodCurrent = Mathf.Clamp(GameCore.instance.foodCurrent + foodIncrement * Time.deltaTime, 0, 100);
                    //Debug.Log("Delaying food for " + foodDuration);
                }

                if (elapsed < drinkDuration)
                {
                    GameCore.instance.waterCurrent = Mathf.Clamp(GameCore.instance.waterCurrent + drinkIncrement * Time.deltaTime, 0, 100);
                    //Debug.Log("Delaying drink for " + drinkDuration);
                }

                yield return null;
            }
        }

        private class ConfigData
        {
            public float foodDuration;
            public float drinkDuration;
        }

    }

  ///This mod work by making a pre-patch of the SetStats method inside the GameCore class.
  // It replace the original logic by sending the food and drink values to a coroutine then block the original instant update.
  // it"s not yet tested over multiple failure cases like spamming food and drinks.

    [HarmonyPatch(typeof(GameCore), "SetStats")]
    public class SetStatsPatch
    {
        static bool Prefix(GameCore __instance, float food, float drink, float health, bool overrun)
        {
            FighterData data = Combat_Manager.instance.UserFighter.data;
            data.actualHealth += health;
            if (!overrun && data.actualHealth > data.HealthTotal)
            {
                data.actualHealth = data.HealthTotal;
            }
            if (data.actualHealth < 0f)
            {
                data.actualHealth = 1f;
            }

            if (DelayedFoodSystem.Instance != null)
            {
                DelayedFoodSystem.Instance.StartDelayedFoodSystem(food, drink);
            }

            return false;
        }
    }


}
