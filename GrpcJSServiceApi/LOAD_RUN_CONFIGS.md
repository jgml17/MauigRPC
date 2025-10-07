# Loading Run Configurations in Rider

How to make the pre-created run configurations appear in Rider.

---

## 🎯 Quick Fix: Refresh & Reload

### Step 1: Open the Project
1. Open Rider
2. **File** → **Open...**
3. Select: `/Users/jgmlsolucoes/Projetos/POC/MauigRPC/GrpcJSServiceApi`
4. Click **Open**

### Step 2: Check for Configurations
Look at the **top-right corner** of Rider. You should see:
```
▼ [Dropdown showing run configurations]
```

If you see configurations named:
- **GrpcJSServiceApi (Dev)**
- **GrpcJSServiceApi (Prod)**
- **GrpcJSServiceApi (Build)**

✅ **They're loaded!** You're done!

---

## 🔧 If Configurations Don't Appear

### Method A: Invalidate Caches (Quick Fix)

1. **File** → **Invalidate Caches...**
2. Select **"Invalidate and Restart"**
3. Wait for Rider to restart
4. Configurations should appear

### Method B: Create Them Manually (5 minutes)

If they still don't appear, create them manually:

#### 1. Open Run Configurations
- **Run** → **Edit Configurations...**
- Or click dropdown at top-right → **Edit Configurations...**

#### 2. Create Dev Configuration

1. Click **+** (Add New Configuration)
2. Select **npm**
3. Fill in:
   - **Name:** `GrpcJSServiceApi (Dev)`
   - **package.json:** Click **folder icon** → Navigate to:
     `/Users/jgmlsolucoes/Projetos/POC/MauigRPC/GrpcJSServiceApi/package.json`
   - **Command:** `run`
   - **Scripts:** `start:dev`
   - **Node interpreter:** (auto-detect)
4. Click **Apply**

#### 3. Create Prod Configuration

1. Click **+** → **npm**
2. Fill in:
   - **Name:** `GrpcJSServiceApi (Prod)`
   - **package.json:** (same as above)
   - **Command:** `run`
   - **Scripts:** `start:prod`
3. Click **Apply**

#### 4. Create Build Configuration

1. Click **+** → **npm**
2. Fill in:
   - **Name:** `GrpcJSServiceApi (Build)`
   - **package.json:** (same as above)
   - **Command:** `run`
   - **Scripts:** `build`
3. Click **Apply**

#### 5. Done!
Click **OK** to close the dialog.

---

## 📊 Visual Guide: Creating npm Configuration

Here's exactly what to fill in:

```
┌─────────────────────────────────────────┐
│ Run/Debug Configurations                │
├─────────────────────────────────────────┤
│ Name: [GrpcJSServiceApi (Dev)        ]  │
│                                          │
│ Configuration:                           │
│   package.json:                          │
│   [📁] /Users/.../GrpcJSServiceApi/...  │
│                                          │
│   Command: [run ▼]                      │
│                                          │
│   Scripts: [start:dev ▼]               │
│                                          │
│   Arguments: [                     ]     │
│                                          │
│   Node interpreter:                      │
│   [Project ▼]                           │
│                                          │
│   Environment variables:                 │
│   [                                 ]    │
│                                          │
│   Working directory:                     │
│   [/Users/.../GrpcJSServiceApi      ]   │
│                                          │
│ [Apply] [OK] [Cancel]                   │
└─────────────────────────────────────────┘
```

---

## 🎮 Using the Configurations

Once created, you'll see them in the dropdown:

### Top-Right Corner of Rider:
```
┌──────────────────────────────────────┐
│ ▼ GrpcJSServiceApi (Dev)       ▶️ 🐛 │
└──────────────────────────────────────┘
```

### Click the Dropdown to See All:
```
▼ Run Configuration
  ├── GrpcJSServiceApi (Dev)   ← Hot reload
  ├── GrpcJSServiceApi (Prod)  ← Production
  └── GrpcJSServiceApi (Build) ← Build only
```

### To Run:
1. Select configuration from dropdown
2. Click **▶️** (Run) or **🐛** (Debug)

---

## 🚀 Quick Test

After loading configurations:

### 1. Select "GrpcJSServiceApi (Dev)"

### 2. Click ▶️ Run

### 3. You Should See:
```
> GrpcJSServiceApi@0.0.1 start:dev
> nest start --watch

[Nest] 12345  - 10/07/2025, 7:25:00 PM     LOG [NestFactory] Starting Nest application...
[Nest] 12345  - 10/07/2025, 7:25:00 PM     LOG [InstanceLoader] AppModule dependencies initialized +10ms
[Nest] 12345  - 10/07/2025, 7:25:00 PM     LOG [NestMicroservice] Nest microservice successfully started +5ms

🚀 gRPC Server is running on http://localhost:5195

Available Services:
  - greet.Greeter
  - contact.ContactService
```

✅ **Success!**

---

## 🐛 Troubleshooting

### Issue: "package.json not found"

**Fix:** Make sure you browse to the correct path:
```
/Users/jgmlsolucoes/Projetos/POC/MauigRPC/GrpcJSServiceApi/package.json
```

Not:
```
/Users/jgmlsolucoes/Projetos/POC/MauigRPC/package.json ❌ (wrong!)
```

### Issue: "Cannot find module @nestjs/core"

**Fix:** Install dependencies first:
```bash
cd /Users/jgmlsolucoes/Projetos/POC/MauigRPC/GrpcJSServiceApi
npm install
```

### Issue: Scripts dropdown is empty

**Fix:** Rider is reading the wrong `package.json`. Click the folder icon and select the correct one.

### Issue: "Node interpreter not configured"

**Fix:**
1. **Rider** → **Preferences** (`Cmd+,`)
2. **Languages & Frameworks** → **Node.js**
3. Click **Configure** → Select Node.js
4. If not installed: `brew install node`

---

## 📁 Configuration Files Location

The run configurations are stored here:
```
GrpcJSServiceApi/
└── .idea/
    └── runConfigurations/
        ├── GrpcJSServiceApi__Dev_.xml
        ├── GrpcJSServiceApi__Prod_.xml
        └── GrpcJSServiceApi__Build_.xml
```

Rider automatically reads these when you open the project!

---

## 🎯 Alternative: Use npm Scripts Panel

If you don't want to use run configurations, you can use the npm Scripts panel:

### Step 1: Show npm Scripts
1. Right-click `package.json`
2. Select **"Show npm Scripts"**

### Step 2: Panel Appears on Right
You'll see all scripts from `package.json`:
```
npm
├── start
├── start:dev    ← Double-click this!
├── start:prod
└── build
```

### Step 3: Run Any Script
- **Double-click** any script to run it
- Right-click → **Run** or **Debug**

---

## 💡 Pro Tip: Pin npm Scripts Panel

Keep the npm Scripts panel visible:

1. Right-click `package.json` → **Show npm Scripts**
2. Panel opens on the right
3. Click the **📌 pin icon** at top of panel
4. Panel stays open permanently

Now you always have quick access to all scripts!

---

## 🎉 Summary

### Best Method for You:

**Option 1:** Open `GrpcJSServiceApi` as a project
- Configurations auto-load ✅
- Everything just works

**Option 2:** Use npm Scripts panel
- Right-click `package.json` → Show npm Scripts
- Double-click `start:dev`
- No configuration needed!

**Option 3:** Create run configurations manually
- Follow steps above
- Full control over settings

---

## 📚 Next Steps

Once configurations are loaded:

1. ✅ **Select "GrpcJSServiceApi (Dev)"**
2. ✅ **Click ▶️ Run**
3. ✅ **Server starts on port 5195**
4. ✅ **Test with grpcurl**
5. ✅ **Make code changes** (hot reload works!)

Happy coding! 🚀
