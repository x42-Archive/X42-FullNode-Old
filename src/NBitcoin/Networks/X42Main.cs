using System;
using System.Collections.Generic;
using System.Linq;
using NBitcoin.BouncyCastle.Math;
using NBitcoin.DataEncoders;
using NBitcoin.Protocol;

namespace NBitcoin.Networks
{
    public class x42Main : Network
    {

        /// <summary> x42 maximal value for the calculated time offset. If the value is over this limit, the time syncing feature will be switched off. </summary>
        public const int x42MaxTimeOffsetSeconds = 25 * 60;

        /// <summary> x42 default value for the maximum tip age in seconds to consider the node in initial block download (2 hours). </summary>
        public const int x42DefaultMaxTipAgeInSeconds = 2 * 60 * 60;

        /// <summary> The name of the root folder containing the different x42 blockchains (x42Main, x42Test, x42RegTest). </summary>
        public const string x42RootFolderName = "x42";

        /// <summary> The default name used for the x42 configuration file. </summary>
        public const string x42DefaultConfigFilename = "x42.conf";

        public x42Main()
        {
            // The message start string is designed to be unlikely to occur in normal data.
            // The characters are rarely used upper ASCII, not valid as UTF-8, and produce
            // a large 4-byte int at any alignment.
            var messageStart = new byte[4];
            messageStart[0] = 0x70;
            messageStart[1] = 0x35;
            messageStart[2] = 0x22;
            messageStart[3] = 0x05;
            uint magic = BitConverter.ToUInt32(messageStart, 0); //0x5223570; 

            this.Name = "x42Main";
            this.Magic = magic;
            this.DefaultPort = 52342;
            this.RPCPort = 52343;
            this.MaxTipAge = 2 * 60 * 60;
            this.MinTxFee = Money.Zero;
            this.FallbackFee = 1000;
            this.MinRelayTxFee = 0;
            this.RootFolderName = x42RootFolderName;
            this.DefaultConfigFilename = x42DefaultConfigFilename;
            this.MaxTimeOffsetSeconds = 25 * 60;
            this.CoinTicker = "x42";

            this.Consensus.SubsidyHalvingInterval = 210000;
            this.Consensus.MajorityEnforceBlockUpgrade = 750;
            this.Consensus.MajorityRejectBlockOutdated = 950;
            this.Consensus.MajorityWindow = 1000;
            this.Consensus.BuriedDeployments[BuriedDeployments.BIP34] = 0;
            this.Consensus.BuriedDeployments[BuriedDeployments.BIP65] = 0;
            this.Consensus.BuriedDeployments[BuriedDeployments.BIP66] = 0;
            this.Consensus.BIP34Hash = new uint256("0x000000000000024b89b42a942fe0d9fea3bb44ab7bd1b19115dd6a759c0808b8");
            this.Consensus.PowLimit = new Target(new uint256("00000fffffffffffffffffffffffffffffffffffffffffffffffffffffffffff"));
            this.Consensus.PowTargetTimespan = TimeSpan.FromSeconds(14 * 24 * 60 * 60); // two weeks
            this.Consensus.PowTargetSpacing = TimeSpan.FromSeconds(10 * 60);
            this.Consensus.PowAllowMinDifficultyBlocks = false;
            this.Consensus.PowNoRetargeting = false;
            this.Consensus.RuleChangeActivationThreshold = 1916; // 95% of 2016
            this.Consensus.MinerConfirmationWindow = 2016; // nPowTargetTimespan / nPowTargetSpacing
            this.Consensus.LastPOWBlock = 523;
            this.Consensus.IsProofOfStake = true;
            this.Consensus.ConsensusFactory = new PosConsensusFactory() { Consensus = this.Consensus };
            this.Consensus.ProofOfStakeLimitV2 = new BigInteger(uint256.Parse("000000000000ffffffffffffffffffffffffffffffffffffffffffffffffffff").ToBytes(false));
            this.Consensus.CoinType = 424242;
            this.Consensus.DefaultAssumeValid = null; //new uint256("0x6b86db75dfc2c36cbb104f5daaf37cf0a5252505a0d178aaab91a2513c08db0a"); // 795970
            this.Consensus.CoinbaseMaturity = 100;
            this.Consensus.PremineReward = Money.Coins(10.5m * 1000000);
            this.Consensus.PremineHeight = 2;
            this.Consensus.ProofOfWorkReward = Money.Zero;
            this.Consensus.ProofOfStakeReward = Money.Coins(20);
            this.Consensus.ProofOfStakeRewardAfterSubsidyLimit = Money.Coins(5);
            this.Consensus.MaxReorgLength = 500;
            this.Consensus.MaxMoney = Money.Coins(42 * 1000000);
            this.Consensus.SubsidyLimit = 550000;


            this.Base58Prefixes = new byte[12][];
            this.Base58Prefixes[(int)Base58Type.PUBKEY_ADDRESS] = new byte[] { (75) };
            this.Base58Prefixes[(int)Base58Type.SCRIPT_ADDRESS] = new byte[] { (125) };
            this.Base58Prefixes[(int)Base58Type.SECRET_KEY] = new byte[] { (75 + 128) };
            this.Base58Prefixes[(int)Base58Type.ENCRYPTED_SECRET_KEY_NO_EC] = new byte[] { 0x01, 0x42 };
            this.Base58Prefixes[(int)Base58Type.ENCRYPTED_SECRET_KEY_EC] = new byte[] { 0x01, 0x43 };
            this.Base58Prefixes[(int)Base58Type.EXT_PUBLIC_KEY] = new byte[] { (0x04), (0x88), (0xB2), (0x1E) };
            this.Base58Prefixes[(int)Base58Type.EXT_SECRET_KEY] = new byte[] { (0x04), (0x88), (0xAD), (0xE4) };
            this.Base58Prefixes[(int)Base58Type.PASSPHRASE_CODE] = new byte[] { 0x2C, 0xE9, 0xB3, 0xE1, 0xFF, 0x39, 0xE2 };
            this.Base58Prefixes[(int)Base58Type.CONFIRMATION_CODE] = new byte[] { 0x64, 0x3B, 0xF6, 0xA8, 0x9A };
            this.Base58Prefixes[(int)Base58Type.STEALTH_ADDRESS] = new byte[] { 0x2a };
            this.Base58Prefixes[(int)Base58Type.ASSET_ID] = new byte[] { 23 };
            this.Base58Prefixes[(int)Base58Type.COLORED_ADDRESS] = new byte[] { 0x13 };

            this.Checkpoints = new Dictionary<int, CheckpointInfo>();
            this.DNSSeeds = new List<DNSSeedData>
            {
                //new DNSSeedData("test.x42.tech", "test.x42.tech"),
                new DNSSeedData("test2.x42.tech", "test2.x42.tech")
            };

            string[] seedNodes = { "192.168.0.2", "192.168.0.1" };
            this.SeedNodes = ConvertToNetworkAddresses(seedNodes, this.DefaultPort).ToList();
                       
            // Create the genesis block.
            this.GenesisTime = 1470467000;
            this.GenesisNonce = 246101626;
            this.GenesisBits = this.Consensus.PowLimit;
            this.GenesisVersion = 1;
            this.GenesisReward = Money.Zero;

            this.Genesis = CreateX42GenesisBlock(this.Consensus.ConsensusFactory, this.GenesisTime, this.GenesisNonce, this.GenesisBits, this.GenesisVersion, this.GenesisReward);
            this.Consensus.HashGenesisBlock = this.Genesis.GetHash();
            Assert(this.Consensus.HashGenesisBlock == uint256.Parse("0x896a04755a27e4765234c21bb92889396b29bc17892713dd60a28fed5f3e89ee"));
            Assert(this.Genesis.Header.HashMerkleRoot == uint256.Parse("0x4468909b6f805dc4b15586463b74df01615d76ea4ba1a3a776d8960ef321f600"));
        }

        protected static Block CreateX42GenesisBlock(ConsensusFactory consensusFactory, uint nTime, uint nNonce, uint nBits, int nVersion, Money genesisReward)
        {
            string pszTimestamp = "Some people trapped in ‘modern slavery’ are underworked – and they pay a heavy price for it - http://theconversation.com/some-people-trapped-in-modern-slavery-are-underworked-and-they-pay-a-heavy-price-for-it-99863 | popó & lita - 6F3582CC2B720980C936D95A2E07F809";

            Transaction txNew = consensusFactory.CreateTransaction();
            txNew.Version = 1;
            txNew.Time = nTime;
            txNew.AddInput(new TxIn()
            {
                ScriptSig = new Script(Op.GetPushOp(0), new Op()
                {
                    Code = (OpcodeType)0x1,
                    PushData = new[] { (byte)42 }
                }, Op.GetPushOp(Encoders.ASCII.DecodeData(pszTimestamp)))
            });
            txNew.AddOutput(new TxOut()
            {
                Value = genesisReward,
            });
            Block genesis = consensusFactory.CreateBlock();
            genesis.Header.BlockTime = Utils.UnixTimeToDateTime(nTime);
            genesis.Header.Bits = nBits;
            genesis.Header.Nonce = nNonce;
            genesis.Header.Version = nVersion;
            genesis.Transactions.Add(txNew);
            genesis.Header.HashPrevBlock = uint256.Zero;
            genesis.UpdateMerkleRoot();
            return genesis;
        }
    }
}