# Adding NestJS Project to Your .NET Solution in Rider

Complete guide to manage both .NET and NestJS projects together in Rider.

---

## 🎯 Method 1: Attach Folder (Recommended)

This keeps both projects in one Rider window without modifying the `.sln` file.

### Step-by-Step:

1. **Open Your Existing Solution**
   - Open Rider
   - **File** → **Open** → Select your `MauigRPC.sln` file

2. **Attach the NestJS Folder**
   - In the **Solution Explorer** (left panel)
   - Right-click on the solution name at the top
   - Select **Attach Existing Folder...**
   - Navigate to: `/Users/jgmlsolucoes/Projetos/POC/MauigRPC/GrpcJSServiceApi`
   - Click **OK**

3. **Result**
   ```
   Solution 'MauigRPC'
   ├── GrpcServiceApi (C# .NET)
   ├── MauigRPC (MAUI App)
   └── GrpcJSServiceApi (NestJS) ← Attached folder
   ```

### Benefits:
- ✅ Both projects visible in one window
- ✅ No modification to `.sln` file
- ✅ Independent build systems
- ✅ Can run both services simultaneously

---

## 🎯 Method 2: Solution Folder (Visual Organization)

Add a solution folder to organize both projects logically.

### Step-by-Step:

1. **Open Your Solution** in Rider

2. **Create a Solution Folder**
   - Right-click solution name
   - **Add** → **New Solution Folder**
   - Name it: `Services` or `Backend`

3. **Attach NestJS Project**
   - Right-click the new folder
   - **Attach Existing Folder...**
   - Select `GrpcJSServiceApi` folder

4. **Result**
   ```
   Solution 'MauigRPC'
   ├── Services/
   │   ├── GrpcServiceApi (C# .NET)
   │   └── GrpcJSServiceApi (NestJS)
   └── MauigRPC (MAUI App)
   ```

---

## 🎯 Method 3: Open Root Folder

Open the entire `MauigRPC` folder as a workspace.

### Step-by-Step:

1. **Close Current Solution** (if open)

2. **Open Folder**
   - **File** → **Open...**
   - Navigate to: `/Users/jgmlsolucoes/Projetos/POC/MauigRPC` (root folder)
   - Click **Open**

3. **Rider Detects Everything**
   - Finds the `.sln` file
   - Detects NestJS `package.json`
   - Shows both projects

4. **Result**
   ```
   MauigRPC (Folder)
   ├── GrpcServiceApi/
   ├── GrpcJSServiceApi/
   └── MauigRPC/
   ```

### Benefits:
- ✅ Everything in one place
- ✅ No configuration needed
- ✅ Rider automatically recognizes both project types

---

## 🔧 Create Compound Run Configuration

Run both .NET and NestJS services together with one click!

### Step-by-Step:

1. **Open Run Configurations**
   - **Run** → **Edit Configurations...**

2. **Create Compound Configuration**
   - Click **+** → **Compound**
   - **Name:** `All Services`

3. **Add Configurations**
   - Click **+** in the right panel
   - Add: `GrpcServiceApi` (your .NET service)
   - Add: `GrpcJSServiceApi (Dev)` (NestJS service)

4. **Configure Options**
   - Check **"Run configurations in parallel"**
   - Optionally check **"Store as project file"**

5. **Apply & OK**

### Now You Can:
- Select **"All Services"** from run dropdown
- Click **▶️ Run** button
- Both services start simultaneously!
- .NET on port **5194**
- NestJS on port **5195**

---

## 📊 Recommended Project Structure in Rider

After setup, your Solution Explorer should look like:

```
Solution 'MauigRPC'
│
├── 📁 GrpcServiceApi (C#)
│   ├── Program.cs
│   ├── Services/
│   └── Protos/
│
├── 📁 GrpcJSServiceApi (TypeScript)
│   ├── src/
│   ├── package.json
│   └── Documentation/
│
└── 📁 MauigRPC (MAUI)
    ├── MainPage.xaml
    └── MauiProgram.cs
```

---

## 🎮 Managing Multiple Projects

### View Both Project Types

**Solution Explorer View:**
- Shows .NET projects in solution structure
- Shows attached NestJS folder

**Project Tool Window:**
- **View** → **Tool Windows** → **Project**
- See file system view of all projects

### Run Configurations Dropdown

You'll see:
```
▼ Run Configuration Dropdown
  ├── GrpcServiceApi (.NET)
  ├── MauigRPC (MAUI)
  ├── GrpcJSServiceApi (Dev) (NestJS)
  ├── GrpcJSServiceApi (Prod) (NestJS)
  └── All Services (Compound) ⭐
```

### Terminal Management

Open multiple terminals:
- **Terminal 1:** .NET project commands
- **Terminal 2:** NestJS project commands (`cd GrpcJSServiceApi`)
- Switch with tabs at bottom

---

## 🚀 Quick Actions After Setup

### 1. Run Both Services
```
Select: "All Services" → Click ▶️
```

### 2. Test Both Services
```bash
# Terminal in Rider
# Test .NET service (port 5194)
grpcurl -plaintext -d '{"name": "World"}' localhost:5194 greet.Greeter/SayHello

# Test NestJS service (port 5195)
grpcurl -plaintext -d '{"name": "World"}' localhost:5195 greet.Greeter/SayHello
```

### 3. Switch Between Projects
- `Cmd+Shift+O` → Type filename to jump
- Works across both .NET and NestJS files!

---

## 📁 Folder Configuration File

If you want to persist the folder attachment, Rider stores this in `.idea/` folder automatically.

The configuration is saved in:
```
.idea/
├── workspace.xml          # Attached folders
└── runConfigurations/     # Run configurations
```

This is automatically shared when you commit (if you include `.idea/`).

---

## 🎯 Practical Example

Here's what I do:

### My Setup:
1. Open `MauigRPC.sln` in Rider
2. Attach `GrpcJSServiceApi` folder
3. Create compound run configuration "All Services"
4. Pin npm Scripts panel for quick access

### My Workflow:
1. Click **"All Services"** → Both start
2. Make changes in C# or TypeScript
3. Both have hot reload (dotnet watch & nest watch)
4. Test both services from same terminal
5. Debug either service with breakpoints

---

## 🐛 Debugging Both Services

### Set Breakpoints in Both:
1. **C# Service:** `GreeterService.cs` line 43
2. **NestJS Service:** `greeter.controller.ts` line 48

### Run in Debug Mode:
- Use compound configuration in debug mode
- Call services with grpcurl
- Rider stops at breakpoints in both!

---

## ⚙️ Advanced: Solution Configuration

You can also add the NestJS folder to `.sln` file manually, but it's not necessary.

### Optional: Add to .sln File

Edit `MauigRPC.sln` and add:

```
Project("{2150E333-8FDC-42A3-9474-1A3956D46DE8}") = "GrpcJSServiceApi", "GrpcJSServiceApi", "{GUID}"
EndProject

Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "Solution Items", "Solution Items", "{GUID}"
    ProjectSection(SolutionItems) = preProject
        GrpcJSServiceApi\package.json = GrpcJSServiceApi\package.json
        GrpcJSServiceApi\README.md = GrpcJSServiceApi\README.md
    EndProjectSection
EndProject
```

**But this is optional!** Method 1 (Attach Folder) works perfectly without modifying `.sln`.

---

## 🚨 Common Questions

### Q: Will this mess up my .NET solution?
**A:** No! Attaching a folder doesn't modify the `.sln` file.

### Q: Can I build both with one command?
**A:** They use different build systems:
- .NET: `dotnet build`
- NestJS: `npm run build`

Use the compound run configuration to run both!

### Q: Can I commit this configuration?
**A:** Yes! The `.idea/` folder contains the configuration. You can:
- Commit it: Everyone gets the same setup
- Ignore it: Everyone configures separately

### Q: Will Git see both projects?
**A:** Yes, they're both in the same repo:
```
MauigRPC/
├── .git/
├── GrpcServiceApi/
├── GrpcJSServiceApi/
└── MauigRPC/
```

---

## 🎉 Success!

After setup, you'll have:

✅ Both projects visible in one Rider window  
✅ Run configurations for each service  
✅ Compound configuration to run both  
✅ Separate terminals for each project  
✅ IntelliSense for C# and TypeScript  
✅ Debugging for both services  

---

## 📚 Next Steps

1. ✅ **Attach folder** (Method 1)
2. ✅ **Create compound run configuration**
3. ✅ **Run both services**
4. ✅ **Test both with grpcurl**
5. ✅ **Try debugging both**

Happy multi-project development! 🚀
