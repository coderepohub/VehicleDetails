using Newtonsoft.Json;

namespace VehicleDetails.DomainModel
{

    public class RDWApiVehicleDataResponse
    {
        [JsonProperty("kenteken")]
        public string Kenteken { get; set; }

        [JsonProperty("voertuigsoort")]
        public string Voertuigsoort { get; set; }

        [JsonProperty("merk")]
        public string Merk { get; set; }

        [JsonProperty("handelsbenaming")]
        public string Handelsbenaming { get; set; }

        [JsonProperty("vervaldatum_apk")]
        public string VervaldatumApk { get; set; }

        [JsonProperty("datum_tenaamstelling")]
        public string DatumTenaamstelling { get; set; }

        [JsonProperty("bruto_bpm")]
        public string BrutoBpm { get; set; }

        [JsonProperty("inrichting")]
        public string Inrichting { get; set; }

        [JsonProperty("aantal_zitplaatsen")]
        public string AantalZitplaatsen { get; set; }

        [JsonProperty("eerste_kleur")]
        public string EersteKleur { get; set; }

        [JsonProperty("tweede_kleur")]
        public string TweedeKleur { get; set; }

        [JsonProperty("aantal_cilinders")]
        public string AantalCilinders { get; set; }

        [JsonProperty("cilinderinhoud")]
        public string Cilinderinhoud { get; set; }

        [JsonProperty("massa_ledig_voertuig")]
        public string MassaLedigVoertuig { get; set; }

        [JsonProperty("toegestane_maximum_massa_voertuig")]
        public string ToegestaneMaximumMassaVoertuig { get; set; }

        [JsonProperty("massa_rijklaar")]
        public string MassaRijklaar { get; set; }

        [JsonProperty("datum_eerste_toelating")]
        public string DatumEersteToelating { get; set; }

        [JsonProperty("datum_eerste_tenaamstelling_in_nederland")]
        public string DatumEersteTenaamstellingInNederland { get; set; }

        [JsonProperty("wacht_op_keuren")]
        public string WachtOpKeuren { get; set; }

        [JsonProperty("catalogusprijs")]
        public string Catalogusprijs { get; set; }

        [JsonProperty("wam_verzekerd")]
        public string WamVerzekerd { get; set; }

        [JsonProperty("aantal_deuren")]
        public string AantalDeuren { get; set; }

        [JsonProperty("aantal_wielen")]
        public string AantalWielen { get; set; }

        [JsonProperty("afstand_hart_koppeling_tot_achterzijde_voertuig")]
        public string AfstandHartKoppelingTotAchterzijdeVoertuig { get; set; }

        [JsonProperty("afstand_voorzijde_voertuig_tot_hart_koppeling")]
        public string AfstandVoorzijdeVoertuigTotHartKoppeling { get; set; }

        [JsonProperty("lengte")]
        public string Lengte { get; set; }

        [JsonProperty("breedte")]
        public string Breedte { get; set; }

        [JsonProperty("europese_voertuigcategorie")]
        public string EuropeseVoertuigcategorie { get; set; }

        [JsonProperty("plaats_chassisnummer")]
        public string PlaatsChassisnummer { get; set; }

        [JsonProperty("technische_max_massa_voertuig")]
        public string TechnischeMaxMassaVoertuig { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("typegoedkeuringsnummer")]
        public string Typegoedkeuringsnummer { get; set; }

        [JsonProperty("variant")]
        public string Variant { get; set; }

        [JsonProperty("uitvoering")]
        public string Uitvoering { get; set; }

        [JsonProperty("volgnummer_wijziging_eu_typegoedkeuring")]
        public string VolgnummerWijzigingEuTypegoedkeuring { get; set; }

        [JsonProperty("vermogen_massarijklaar")]
        public string VermogenMassarijklaar { get; set; }

        [JsonProperty("wielbasis")]
        public string Wielbasis { get; set; }

        [JsonProperty("export_indicator")]
        public string ExportIndicator { get; set; }

        [JsonProperty("openstaande_terugroepactie_indicator")]
        public string OpenstaandeTerugroepactieIndicator { get; set; }

        [JsonProperty("taxi_indicator")]
        public string TaxiIndicator { get; set; }

        [JsonProperty("maximum_massa_samenstelling")]
        public string MaximumMassaSamenstelling { get; set; }

        [JsonProperty("aantal_rolstoelplaatsen")]
        public string AantalRolstoelplaatsen { get; set; }

        [JsonProperty("maximum_ondersteunende_snelheid")]
        public string MaximumOndersteunendeSnelheid { get; set; }

        [JsonProperty("jaar_laatste_registratie_tellerstand")]
        public string JaarLaatsteRegistratieTellerstand { get; set; }

        [JsonProperty("tellerstandoordeel")]
        public string Tellerstandoordeel { get; set; }

        [JsonProperty("code_toelichting_tellerstandoordeel")]
        public string CodeToelichtingTellerstandoordeel { get; set; }

        [JsonProperty("tenaamstellen_mogelijk")]
        public string TenaamstellenMogelijk { get; set; }

        [JsonProperty("vervaldatum_apk_dt")]
        public string VervaldatumApkDt { get; set; }

        [JsonProperty("datum_tenaamstelling_dt")]
        public string DatumTenaamstellingDt { get; set; }

        [JsonProperty("datum_eerste_toelating_dt")]
        public string DatumEersteToelatingDt { get; set; }

        [JsonProperty("datum_eerste_tenaamstelling_in_nederland_dt")]
        public string DatumEersteTenaamstellingInNederlandDt { get; set; }

        [JsonProperty("zuinigheidsclassificatie")]
        public string Zuinigheidsclassificatie { get; set; }

        [JsonProperty("api_gekentekende_voertuigen_assen")]
        public string ApiGekentekendeVoertuigenAssen { get; set; }

        [JsonProperty("api_gekentekende_voertuigen_brandstof")]
        public string ApiGekentekendeVoertuigenBrandstof { get; set; }

        [JsonProperty("api_gekentekende_voertuigen_carrosserie")]
        public string ApiGekentekendeVoertuigenCarrosserie { get; set; }

        [JsonProperty("api_gekentekende_voertuigen_carrosserie_specifiek")]
        public string ApiGekentekendeVoertuigenCarrosserieSpecifiek { get; set; }

        [JsonProperty("api_gekentekende_voertuigen_voertuigklasse")]
        public string ApiGekentekendeVoertuigenVoertuigklasse { get; set; }
    }

}