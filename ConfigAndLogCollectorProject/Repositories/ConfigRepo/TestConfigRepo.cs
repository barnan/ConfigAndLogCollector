using BaseClasses;
using Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigAndLogCollectorProject.Repositories.ConfigRepo
{
    public class TestConfigRepo : ConfigRepoBase
    {

        public TestConfigRepo()
            : base()
        {

        }


        public override bool Init()
        {
            ArchiveOption aOpt1 = new ArchiveOption("AOption1", new List<ArchPath> { new ArchPath("*.*", true, 10) });
            aOpt1.IsSelected = true;
            ArchiveOption aOpt2 = new ArchiveOption("AOption2", new List<ArchPath> { new ArchPath("*.xml", true, 5), new ArchPath("*.config", true, 5) });
            ArchiveOption aOpt3 = new ArchiveOption("AOption3", new List<ArchPath> { new ArchPath("*.config", false, 5), new ArchPath("*.txt", false, 10) });

            ArchiveOption aOpt4 = new ArchiveOption("AOption4", new List<ArchPath> { new ArchPath("*.log", true, 10) });
            ArchiveOption aOpt5 = new ArchiveOption("AOption5", new List<ArchPath> { new ArchPath("*.exe", true, 5), new ArchPath("*.config", true, 5) });
            ArchiveOption aOpt6 = new ArchiveOption("AOption6", new List<ArchPath> { new ArchPath("*.dll", false, 5) });


            ArchiveOptions = new ArchiveConfigs { OptionList = new List<ArchiveOption> { aOpt1, aOpt2, aOpt3, aOpt4, aOpt5, aOpt6 } };

            return IsInitialized = true;
        }

        public override void Close()
        {
        }


        public override bool Save(ArchiveOption element)
        {
            throw new NotImplementedException();
        }


    }
}
