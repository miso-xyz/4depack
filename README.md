# 4depack
Library to play around with 4dots packers

## Changes from VB.NET
<pre>
- Support for CRC32 Hash Generation & Check
- Improved Project File Reading
- AutoFind Decryption & Encryption Keys
- Known Decryption Keys Available (FourDepack_cs.Security.CryptoHelper.KnownDecryptionKeys)
- Resource Extraction Optimised
</pre>

## To Do:
- Fix Unstable Project Reading `(project.xml/project.zsp)`

## Code Samples
<details>
 <summary>Save all resources</summary>
 <hr></hr>
  
  ```csharp
  foreach (AppResources resource in app.TargetApplicationResources)
  {
     byte[] tempData = resource.RawData;
     File.WriteAllBytes("4depack\\" + Path.GetFileName(args[0]) + "\\" + resource.Filename, tempData);
  }
  ```
  
  <hr></hr>
</details>
<details>
 <summary>Try decrypting all resources with found keys</summary>
 <hr></hr>
  
  ```csharp
  foreach (AppResources resource in app.TargetApplicationResources)
  {
     byte[] tempData = resource.RawData;
     string[] keys = FourDepack_cs.Security.CryptoHelper.FindKeys(app, FourDepack_cs.Security.CryptoHelper.SearchType.DecryptionKeys);
     foreach (string key in keys)
     {
         try { tempData = FourDepack_cs.Security.CryptoHelper.DecryptBytes(tempData, key); }
         catch { continue; }
     }
     File.WriteAllBytes("4depack\\" + Path.GetFileName(args[0]) + "\\" + resource.Filename, tempData);
  }
  ```
  
  <hr></hr>
</details>

# Credits
- 0xd4d (now wtfsck) - [dnlib](https://github.com/0xd4d/dnlib)
