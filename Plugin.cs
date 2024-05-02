using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace CardOnly
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded! Applying patch...");
            Harmony harmony = new Harmony("com.orpticon.CardOnly");
            harmony.PatchAll();
        }
    }
    public static class PaymentFixerPatch
    {
        [HarmonyPatch(typeof(Customer), "DoPayment")]
        public static class Customer_DoPayment_Patch
        {
            public static void Prefix(ref bool viaCreditCard)
            {
                viaCreditCard = true;
            }
        }
        [HarmonyPatch(typeof(Checkout), "ChangeState")]
        public static class Checkout_ChangeState_Patch
        {
            public static void Prefix(ref Checkout.State newState)
            {
                if (newState == Checkout.State.CUSTOMER_HANDING_CASH) newState = Checkout.State.CUSTOMER_HANDING_CARD;
                if (newState == Checkout.State.PAYMENT_CASH) newState = Checkout.State.PAYMENT_CREDIT_CARD;
            }
        }
    }
}
