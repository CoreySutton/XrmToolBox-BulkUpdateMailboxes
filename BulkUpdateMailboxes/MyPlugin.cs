using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace CoreySutton.XrmToolBox.BulkUpdateMailboxes
{
    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "Bulk Update Mailboxes"),
        ExportMetadata("Description", "Bulk Update Mailboxes"),
        ExportMetadata("SmallImageBase64", "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAH5SURBVFhH7Za/SxxREMe/8QeeomIhaMCA9goWtkKsxEqbFP4Fh0UggkUawcI/QUiaYGFhY2lno6SwsLGwV9DiAoKSi+FAUT+zjrJ7eue+Pe9Q8APDzHv7dmZ25u3b1TtvmqL0GVm6kAZ8qnHcSLl/0hlyg6z5dDBNroMpSf2oHrM/SCems5A5gTiX0i83g3mRBNqkKzeDoXpPYz3+Lw2b3SHts/Dqj9TZLfXaHE89QPa/zb6WxlsrtOGvdNonsU1SQuAWVi+wuy99g5kUcfT9jJ5j78bmnxX3s8X9Mx6iOiz+EXcQF163VXQ/To/Lr6URe2U9zAOJFhBgjArs+bASU8gk8i0aBUK75mjjTx8+2oTTrquRx8mO28HwxHk3I8oTGHVdESo0hpNzH2Yh2tj3JBLAeaebFSF4Tccu97e4GfEi50AtZEmglvI/IrgFUL8E6E90yjWSN7kHauGANn91OyI4ARwE7QHW25dyAz3BBhvpklburtwRnEDaQ4iAJU7MRdZ/IvAXAm/7pQR1awGBC5z5ywQv+NSTlCfAR6uxJBKwHqE+IlOUcB5tXy0r3SmSwHrLU25i2tfxKJrMAD7SwQ9FL4uHkRzD/WZ0uwembIeoQbNjHPFAQ27XF0vg/qcjJpbUs6SuQDUIto6y3/Q4BSow6/Y7rxXpFgnRri/WpKLwAAAAAElFTkSuQmCC"),
        ExportMetadata("BigImageBase64", "iVBORw0KGgoAAAANSUhEUgAAAFAAAABQCAYAAACOEfKtAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAVNSURBVHhe7ZxPaB1FHMd/Nam0Nq0RBCNaaOCBKbZQsAc9FCJYtIeiQg8RemhPIirag6iIUA9iDwURezAnIwYseCnooQehPXgoWLBgQQXBB+ZQUWjaKAaatn5+b3/Z7Et29/2ZfbuzyXxgmN/sLm9nv/ub2d/OzjwJBAKBAXBD5Ml/RC6Qri+I/EH+1X8iu2x3II+bIkcQ7Rai3V2VFkj77LBAGn+KjCDSXwnR2hLC/mCHesE9lnvDVpFnyB6MSmvZJLIfD83cXzbeCQgTlmcy5FFf6J2AeFjTzFTuiizeJ3LFipXjowdeszwVKjyLyEtWrBwfBcwE77vCg+SEFb2gVgLieZ88JIKG/lArAX0kCOhIENCRIKAjQUBHgoCOEBn0zrxIA+X3kEaJzXaRrpHmKDe3i1y1w/piQWSSSl2w4mqOj4jMmO0FXQtI8DXGwW8g1BGKjWhrKr9x3NlbIl+MYtu2Fogzxb6XrZgKvz/KMVlDVr+Qct9UemSJczU556+ky9ycS5QXbV9XcHw+17mge0XeviPyJgdvsc0doUKLeORp3ls/TFbqpsg7bP/Iir6hQfp56j5NS/ou2pRProDaVIdFvsHsOEKSQxPxD+2IvKcFnqg342Mr+spVhDzRSchMAXVIfSgSr4ixt3lEPIyI31tZ/hX5nAoes6K3INApWtH75KkDGKkCan/Hxf3IzjHbVATaPA7Qz7SGonTkeZvIT5h1+M5xkboeTBOR7qgdhKPVypcFi6egnXytwmnBBgVeUbsGTGqLMbuNNQJy4FEyHVYfBA3Ui/s+7PNkcd/oOUfpu0+aHdPWhNX7EPBnzLwwxQnOsXRbZPdyiEOlXqMSn7Z2eo7WHY8bpznP2aZ2D6RNTZENTDwFsYY3i7xnRe1UZs30Hq072atRKaJNQA541syBwhN5irvZiikfaEVL+d9BfIK6H7PnRIvVfeBzlg8UbtQWAupkP1sbAan7GN3OC1ZcEZAL0mC5tO+t3MLnzdRK1UZAhfj4CTNXBFRlzSwFmkJtp2jQhOPYNSlg2QFtqTesYNYKiEeUekHcsEfNrB1ZHtj1SMtGJ9ndxQIG+iMI6EgQ0JEgoCNVClir4DmL4IGOBAEdiQXEeMTMQA8kPTAeoikJb2aZulBlE45HdetM6AMdCQI6EgR0pDIBN0XfhWtPlR74t+W1JhYwOUgY6J7QBzoSBOwdnfb2utnVCUgl9IN6LaCuizz0Zm6LPDUisne7yBnbVamAN8z0GZ349BZ13blN5Pj9IpeizSuEJrwKxNJ39LPkT+Ntu0mnd+REDEHAFXQq8rs01Z2I9hLN9KJtz2VDC2jedo50CNHG8bRT5D2tAkgKWGocSOWLXK7QE5x7jvQBFz+OYC+SdKJnX1TmgZy4p/UYBTHPk/SwehtN9GRyomS/bLQmPM+T9Fv6ucIGczeagIUTBHSkMgG1Izez1lTpgevuo9K6GOAsm1hAHu17yR6maR0k6WjDGXJduVhZvLYMddCQ5zNSV28HZcITvTO6PAuBJ3jVmUDxx7kgnZDe0Jwf6Ot7Mr91YHnxYc7CQx06mqatz+pyiAIWKDY1BjS7ELoSMAsuZph2P0Gui3NicflRFZi6ZjPEhWy1CUZJYch16Ogcua7ZbfO4dSdgHgir02D3ccENTvIYtgq9B7s1PTZFwEm8cpqbMMNFpnYbPgpYOroCHiH2I0Q8J7vb/wNUAbkxqX/M2GX63X6qMJJP4VLQvox30Mt4YvwunDfe5julC7jeCAI6EgR0JAjoSBDQkYHFgYNAw53NHQL0PIgzl4oYhQ4EfEHkfxu3S4/yJhnXAAAAAElFTkSuQmCC"),
        ExportMetadata("BackgroundColor", "Lavender"),
        ExportMetadata("PrimaryFontColor", "Black"),
        ExportMetadata("SecondaryFontColor", "Gray")]
    public class MyPlugin : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new MyPluginControl();
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        public MyPlugin()
        {
            // If you have external assemblies that you need to load, uncomment the following to 
            // hook into the event that will fire when an Assembly fails to resolve
            // AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveEventHandler);
        }

        /// <summary>
        /// Event fired by CLR when an assembly reference fails to load
        /// Assumes that related assemblies will be loaded from a subfolder named the same as the Plugin
        /// For example, a folder named Sample.XrmToolBox.MyPlugin 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly AssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            Assembly loadAssembly = null;
            Assembly currAssembly = Assembly.GetExecutingAssembly();

            // base name of the assembly that failed to resolve
            var argName = args.Name.Substring(0, args.Name.IndexOf(","));

            // check to see if the failing assembly is one that we reference.
            List<AssemblyName> refAssemblies = currAssembly.GetReferencedAssemblies().ToList();
            var refAssembly = refAssemblies.Where(a => a.Name == argName).FirstOrDefault();

            // if the current unresolved assembly is referenced by our plugin, attempt to load
            if (refAssembly != null)
            {
                // load from the path to this plugin assembly, not host executable
                string dir = Path.GetDirectoryName(currAssembly.Location).ToLower();
                string folder = Path.GetFileNameWithoutExtension(currAssembly.Location);
                dir = Path.Combine(dir, folder);

                var assmbPath = Path.Combine(dir, $"{argName}.dll");

                if (File.Exists(assmbPath))
                {
                    loadAssembly = Assembly.LoadFrom(assmbPath);
                }
                else
                {
                    throw new FileNotFoundException($"Unable to locate dependency: {assmbPath}");
                }
            }

            return loadAssembly;
        }
    }
}