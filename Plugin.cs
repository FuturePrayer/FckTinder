using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BepInEx;
using HarmonyLib;
using System.Reflection;
using System.Reflection.Emit;

namespace FckTinder
{
    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInProcess(GAME_PROCESS)]
    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "cn.suhoan.fcktiner";
        public const string NAME = "FckTiner";
        public const string VERSION = "1.0";
        private const string GAME_PROCESS = "DSPGAME.exe";

        void Start()
        {
            Logger.LogInfo(NAME + " load success!");
        }

        private void Update()
        {
            
            if (GameMain.data?.spaceSector?.dfHives == null) return;
            
            SpaceSector spaceSector = GameMain.data.spaceSector;

            foreach (EnemyDFHiveSystem dfHive in spaceSector.dfHives)
            {
                if (dfHive == null) continue;
                
                for (EnemyDFHiveSystem enemyDfHiveSystem = dfHive;
                     enemyDfHiveSystem != null;
                     enemyDfHiveSystem = enemyDfHiveSystem.nextSibling)
                {
                    if (enemyDfHiveSystem.tinders?.buffer == null) continue;
                    
                    foreach (DFTinderComponent dfTinderComponent in enemyDfHiveSystem.tinders.buffer)
                    {
                        if (dfTinderComponent.id > 0 && dfTinderComponent.enemyId > 0)
                        {
                            if (spaceSector.skillSystem?.combatStats?.buffer == null)
                            {
                                continue;
                            }
                            CombatStat[] combatStats = spaceSector.skillSystem.combatStats.buffer;
                            if (combatStats != null)
                            {
                                Logger.LogInfo("准备销毁火种，enemyId：" + dfTinderComponent.enemyId + "，战斗单位：" +
                                               combatStats[0].id);
                                spaceSector.KillEnemyFinal(dfTinderComponent.enemyId, ref combatStats[0]);
                            }
                        }
                    }
                }
            }
        }
    }
}