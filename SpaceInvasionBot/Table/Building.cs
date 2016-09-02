using System;
using System.Text;

namespace SpaceInvasionBot
{
	enum BuildingType
	{
		IronMine = 1,
		BlastFurnace = 2,
		KryptoniteMine = 3,
		SpiceMine = 4,
		FusionPlant = 5,
		ThermosolarPlant = 6,
		DevelopmentCentre = 7,
		ResearchLab = 8,
		WeaponsFactory = 9,
		FleetBase = 10,
		IronWarehouse = 11,
		MetalWarehouse = 12,
		KryptoniteWarehouse = 13,
		SpiceWarehouse = 14,
		ParticleCannon = 15,
		ParticleShield = 16,
		MicrosystemAccelerator = 17,
		Teleporter = 18,
		GalaxyScanner = 19
	}
	
    class Building
    {
        public int IronMine = 0;
        public int IronWarehouse = 0;
        public int BlastFurnace = 0;
        public int MetalWarehouse = 0;
        public int KryptoniteMine = 0;
        public int KryptoniteWarehouse = 0;
        public int SpiceMine = 0;
        public int SpiceWarehouse = 0;

        public int FusionPlant = 0;
        public int ThermosolarPlant = 0;

        public int DevelopmentCentre = 0;
        public int ResearchLab = 0;
        public int WeaponsFactory = 0;
        public int FleetBase = 0;
        public int ParticleShield = 0;
        public int ParticleCannon = 0;
        public int MicrosystemAccelerator = 0;
        public int Teleporter = 0;
        public int GalaxyScanner = 0;
    }
}
