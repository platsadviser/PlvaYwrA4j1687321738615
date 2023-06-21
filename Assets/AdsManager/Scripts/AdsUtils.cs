using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace EaAds
{
    public class AdsUtils
    {
        public static IEnumerator GetAdConfigFromServer(
            String url,
            Action<DataProgress> progresss,
            Action<AdConfig, bool> result
        )
        {
            var dataProgress = new DataProgress();
            using (var request = UnityWebRequest.Get(url))
            {
                request.certificateHandler = new EaAds.BypassCertificate();
                request.SendWebRequest();
                while (!request.isDone)
                {
                    var progresssPersen = string.Format("{0:0}%", request.downloadProgress * 100);

                    dataProgress.progressPersen = progresssPersen;
                    dataProgress.progressFloat = request.downloadProgress;

                    progresss.Invoke(dataProgress);
                    Debug.Log("progresss: " + request.downloadProgress);
                    yield return null;
                }
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log("Error: " + request.error);
                    result.Invoke(null, false);
                }
                else
                {
                    dataProgress.progressPersen = "100%";
                    dataProgress.progressFloat = 10;
                    progresss.Invoke(dataProgress);
                    var data = JsonConvert.DeserializeObject<AdConfig>(
                        request.downloadHandler.text
                    );
                    result.Invoke(data, true);
                }
            }
        }

        public static void MapConfigAd(AdConfig adConfig)
        {
            if (AdsManagerWrapper.INSTANCE)
            {
                AdsManagerWrapper.INSTANCE.isShowAds = adConfig.IsShowAds;
                AdsManagerWrapper.INSTANCE.isShowOpenAd = adConfig.IsShowOpenAd;
                AdsManagerWrapper.INSTANCE.isShowBanner = adConfig.IsShowBanner;
                AdsManagerWrapper.INSTANCE.isShowInterstitial = adConfig.IsShowInterstitial;
                AdsManagerWrapper.INSTANCE.isShowNativeAd = adConfig.IsShowNativeAd;
                AdsManagerWrapper.INSTANCE.isShowRewards = adConfig.IsShowRewards;

                AdsManagerWrapper.INSTANCE.intervalTimeInterstitial =
                    adConfig.IntervalTimeInterstitial;

                AdsManagerWrapper.INSTANCE.primaryOpenAdId = adConfig.PrimaryOpenAdId;
                AdsManagerWrapper.INSTANCE.secondaryOpenAdId = adConfig.SecondaryOpenAdId;
                AdsManagerWrapper.INSTANCE.tertiaryOpenAdId = adConfig.TertiaryOpenAdId;
                AdsManagerWrapper.INSTANCE.quaternaryOpenAdId = adConfig.QuaternaryOpenAdId;

                AdsManagerWrapper.INSTANCE.primaryAds = adConfig.PrimaryAds;
                AdsManagerWrapper.INSTANCE.secondaryAds = adConfig.SecondaryAds;
                AdsManagerWrapper.INSTANCE.tertiaryAds = adConfig.TertiaryAds;
                AdsManagerWrapper.INSTANCE.quaternaryAds = adConfig.QuaternaryAds;

                AdsManagerWrapper.INSTANCE.primaryAppId = adConfig.PrimaryAppId;
                AdsManagerWrapper.INSTANCE.secondaryAppId = adConfig.SecondaryAppId;
                AdsManagerWrapper.INSTANCE.tertiaryAppId = adConfig.TertiaryAppId;
                AdsManagerWrapper.INSTANCE.quaternaryAppId = adConfig.QuaternaryAppId;

                AdsManagerWrapper.INSTANCE.primaryBannerId = adConfig.PrimaryBannerId;
                AdsManagerWrapper.INSTANCE.secondaryBannerId = adConfig.SecondaryBannerId;
                AdsManagerWrapper.INSTANCE.tertiaryBannerId = adConfig.TertiaryBannerId;
                AdsManagerWrapper.INSTANCE.quaternaryBannerId = adConfig.QuaternaryBannerId;

                AdsManagerWrapper.INSTANCE.primaryInterstitialId = adConfig.PrimaryInterstitialId;
                AdsManagerWrapper.INSTANCE.secondaryInterstitialId =
                    adConfig.SecondaryInterstitialId;
                AdsManagerWrapper.INSTANCE.tertiaryInterstitialId = adConfig.TertiaryInterstitialId;
                AdsManagerWrapper.INSTANCE.quaternaryInterstitialId =
                    adConfig.QuaternaryInterstitialId;

                AdsManagerWrapper.INSTANCE.primaryNativeId = adConfig.PrimaryNativeId;
                AdsManagerWrapper.INSTANCE.secondaryNativeId = adConfig.SecondaryNativeId;
                AdsManagerWrapper.INSTANCE.tertiaryNativeId = adConfig.TertiaryNativeId;
                AdsManagerWrapper.INSTANCE.quaternaryNativeId = adConfig.QuaternaryNativeId;

                AdsManagerWrapper.INSTANCE.primaryRewardsId = adConfig.PrimaryRewardsId;
                AdsManagerWrapper.INSTANCE.secondaryRewardsId = adConfig.SecondaryRewardsId;
                AdsManagerWrapper.INSTANCE.tertiaryRewardsId = adConfig.TertiaryRewardsId;
                AdsManagerWrapper.INSTANCE.quaternaryRewardsId = adConfig.QuaternaryRewardsId;

                AdsManagerWrapper.INSTANCE.testDevices = adConfig.TestDevices;

                EaAds.GlobalValue.applovinSdkKey = adConfig.SdkKeyAppLovin;
            }
        }
    }
}

namespace EaAds
{
    public class BypassCertificate : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            //Simply return true no matter what
            return true;
        }
    }

    public class DataProgress
    {
        public string progressPersen { get; set; }
        public float progressFloat { get; set; }
    }

    public class NetworkAdsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(NetworkAds);
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer
        )
        {
            string value = reader.Value.ToString();

            if (value == "UnityAds")
                return NetworkAds.UNITY_ADS;
            else if (value == "Admob")
                return NetworkAds.ADMOB;
            else if (value == "Fan")
                return NetworkAds.FAN;
            else if (value == "Applovin-Max")
                return NetworkAds.APPLOVIN_MAX;
            else if (value == "Applovin-Discovery")
                return NetworkAds.APPLOVIN_DISCOVERY;
            else if (value == "StartIo")
                return NetworkAds.START_IO;
            else
                return NetworkAds.NONE;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string result;
            NetworkAds networkAdsValue = (NetworkAds)value;

            if (networkAdsValue == NetworkAds.UNITY_ADS)
                result = "UnityAds";
            else if (networkAdsValue == NetworkAds.ADMOB)
                result = "Admob";
            else if (networkAdsValue == NetworkAds.FAN)
                result = "Fan";
            else if (networkAdsValue == NetworkAds.APPLOVIN_MAX)
                result = "Applovin-Max";
            else if (networkAdsValue == NetworkAds.APPLOVIN_DISCOVERY)
                result = "Applovin-Discovery";
            else if (networkAdsValue == NetworkAds.START_IO)
                result = "StartIo";
            else
                result = "None";

            writer.WriteValue(result);
        }
    }
}
