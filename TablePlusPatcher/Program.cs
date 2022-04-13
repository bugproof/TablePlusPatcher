using dnlib.DotNet;
using dnlib.DotNet.Emit;

var filePath = args.Length > 0 ? args[0] : "C:\\Program Files\\TablePlus\\TablePlus.exe";
var bytes = File.ReadAllBytes(filePath);
var module = ModuleDefMD.Load(bytes);
Console.WriteLine($"Patching {module.FullName}");
const string licenseServiceTypeName = "TablePlus.Source.Service.LicenseService";
var licenseServiceTypeDef = module.Find(licenseServiceTypeName, true);
var isLicensedMethodDef = licenseServiceTypeDef.FindMethod("IsLicensed");
var cilBodyInstr = isLicensedMethodDef.Body.Instructions;
cilBodyInstr.Clear();
cilBodyInstr.Add(Instruction.CreateLdcI4(1));
cilBodyInstr.Add(Instruction.Create(OpCodes.Ret));
module.Write(filePath);
Console.WriteLine("Patched. Exiting...");
Thread.Sleep(3000);