# 4depack
Library to play around with 4dots packers

## Changes from VB.NET
<pre>
- Support for CRC32 Hash Generation & Check
- Improved Project File Reading
- AutoFind Decryption & Encryption Keys
- Known Decryption Keys Available (FourDepack.Security.CryptoHelper.KnownDecryptionKeys)
- Resource Extraction Optimised
</pre>

## Works with
Packer Name | Supported Version | Latest Version
------------- | :---: | :---:
Convert Excel to EXE | v2.4 | v2.4
Convert Powerpoint to EXE | v3.5 | v3.5
Convert Word to EXE | v2.4 | v2.4
Standalone EXE Document Locker | v1.1 | v1.1
Standalone EXE Locker | v2.4 | v2.4
ZIP Self Extractor Maker | v1.8 | v1.8

## To Do:
- Change names (some are really long)

## Code Samples
<details>
 <summary>Get passwords (Any supported packer)</summary>
 <hr></hr>
  
  ```csharp
  FourDepack.FourDots_PackedApplication app = new FourDots_PackedApplication(args[0]);
  if (!app.UsesZSPProject())
  {
      if (app.PackedApplication_XMLProject.LockedDocumentProperties != null)
      {
          Console.WriteLine(app.PackedApplication_XMLProject.LockedDocumentProperties.sPassword);
      }
      else
      {
          if (app.PackedApplication_XMLProject.Properties.AskForPassword)
          {
              Console.WriteLine(app.PackedApplication_XMLProject.Properties.Password);
          }
          else { Console.WriteLine("No password"); }
      }
  }
  else
  {
      Console.WriteLine(app.PackedApplication_ZSPProject.MainProperties.EncryptionPassword);
  }
  ```
  
  <hr></hr>
</details>
<details>
 <summary>Dump resources (Any supported packer)</summary>
 <hr></hr>
  
  ```csharp
  foreach (AppResources resource in app.PackedApplication_Resources)
  {
      byte[] tempData = resource.RawData;
      if (app.UsesZSPProject() && resource.Filename.Contains("zipexe.zip"))
      {
          tempData = FourDepack.Security.CryptoHelper.DecryptBytes(tempData, app.PackedApplication_ZSPProject.MainProperties.EncryptionPassword);
      }
      if (!app.UsesZSPProject() && app.PackedApplication_XMLProject.LockedDocumentProperties != null)
      {
          if (resource.Filename.Contains("LockedDocument.rtf"))
          {
              if (app.PackedApplication_XMLProject.LockedDocumentProperties.EncryptFiles)
              {
                  tempData = CryptoHelper.DecryptBytes(resource.RawData, app.PackedApplication_XMLProject.LockedDocumentProperties.sPassword);
              }
              File.WriteAllBytes("4depack\\" + Path.GetFileName(args[0]) + "\\" + resource.Filename + ".bin", tempData);
              return;
          }
      }
      else
      {
          File.WriteAllBytes("4depack\\" + Path.GetFileName(args[0]) + "\\" + resource.Filename + ".bin", tempData);
      }
  }
  ```
  
  <hr></hr>
</details>

# Credits
- 0xd4d (now wtfsck) - [dnlib](https://github.com/0xd4d/dnlib)
