# ✅ Fixed: Configuration Error Resolved!

The "unavailable plugin" error has been fixed!

---

## 🔧 What Was Wrong?

The configurations were using `npm` type, which requires the Node.js plugin. Since you're using Rider for .NET, this plugin might not be enabled by default.

## ✅ What I Fixed

I changed the configurations from **npm type** to **Shell Script type**, which works in all versions of Rider without any plugins.

### Before (Broken):
```xml
<configuration type="js.build_tools.npm">
  <!-- Required Node.js plugin -->
</configuration>
```

### After (Working):
```xml
<configuration type="ShConfigurationType">
  <!-- Built-in shell script support -->
  <option name="SCRIPT_TEXT" value="cd $PROJECT_DIR$/GrpcJSServiceApi &amp;&amp; npm run start:dev" />
</configuration>
```

---

## 🚀 How to Use Them Now

### Step 1: Restart Rider
1. Close Rider completely
2. Reopen your solution: `MauigRPC.sln`

### Step 2: Check Configurations
Look at the **top-right dropdown**. You should see:
```
▼ Run Configurations
  ├── GrpcJSServiceApi (Dev)   ✅ Fixed!
  ├── GrpcJSServiceApi (Prod)  ✅ Fixed!
  └── GrpcJSServiceApi (Build) ✅ Fixed!
```

### Step 3: Install Dependencies (First Time Only)
Open Terminal in Rider (`Option+F12`):
```bash
cd GrpcJSServiceApi
npm install
```

### Step 4: Run It!
1. Select **"GrpcJSServiceApi (Dev)"**
2. Click **▶️ Run**
3. A terminal window opens and runs the service! 🎉

---

## 📊 What Each Configuration Does

### GrpcJSServiceApi (Dev) ⭐
**Command:** `cd GrpcJSServiceApi && npm run start:dev`
- Runs with hot reload
- Auto-restarts on file changes
- Best for development

### GrpcJSServiceApi (Prod)
**Command:** `cd GrpcJSServiceApi && npm run start:prod`
- Production mode
- No hot reload
- Requires build first

### GrpcJSServiceApi (Build)
**Command:** `cd GrpcJSServiceApi && npm run build`
- Compiles TypeScript to JavaScript
- Output goes to `dist/` folder

---

## 🎮 How It Works

When you click Run:
1. Rider opens a **terminal window** inside the IDE
2. Changes to `GrpcJSServiceApi` directory
3. Runs the npm command
4. You see all output in the terminal
5. Click ⏹️ (Stop) to stop the server

### Visual Example:
```
Run Terminal:
┌─────────────────────────────────────────┐
│ $ cd GrpcJSServiceApi && npm run start:dev │
│                                          │
│ > GrpcJSServiceApi@0.0.1 start:dev      │
│ > nest start --watch                    │
│                                          │
│ 🚀 gRPC Server is running on            │
│    http://localhost:5195                │
│                                          │
│ Available Services:                     │
│   - greet.Greeter                       │
│   - contact.ContactService              │
└─────────────────────────────────────────┘
```

---

## 🧪 Quick Test

After starting the server:

### In Rider's Terminal:
```bash
# Test the service
grpcurl -plaintext -d '{"name": "Rider"}' localhost:5195 greet.Greeter/SayHello

# Expected response:
# {
#   "message": "Hello Rider"
# }
```

---

## 💡 Alternative: Use npm Scripts Panel

If you prefer not using run configurations:

### Step 1: Show npm Scripts
1. Find `GrpcJSServiceApi/package.json` in Solution Explorer
2. **Right-click** → **"Show npm Scripts"**
3. Panel opens on the right

### Step 2: Run Any Script
Double-click any script:
```
npm
├── build
├── start
├── start:dev    ← Double-click this!
└── start:prod
```

Same result, no configuration needed!

---

## 🔥 Bonus: Run Both Services Together

Create a compound configuration:

### Steps:
1. **Run** → **Edit Configurations...**
2. Click **+** → **Compound**
3. **Name:** `All Services`
4. Click **+** in right panel, add:
   - Your .NET service configuration
   - `GrpcJSServiceApi (Dev)`
5. Check **"Run configurations in parallel"**
6. Click **OK**

Now run both at once:
- .NET on port **5194**
- NestJS on port **5195**

---

## 🐛 If You Still See Errors

### Error: "npm: command not found"
**Fix:** Node.js not in PATH. In terminal:
```bash
which npm
# Should show: /usr/local/bin/npm or similar
```

If not found, reinstall Node.js:
```bash
brew install node
```

### Error: "Cannot find module @nestjs/core"
**Fix:** Dependencies not installed:
```bash
cd GrpcJSServiceApi
npm install
```

### Error: Configuration still shows as broken
**Fix:**
1. Delete the configuration: **Run** → **Edit Configurations...** → Select it → Click **-**
2. Restart Rider
3. Configurations will reload automatically

---

## 📁 Configuration File Locations

The fixed configurations are here:
```
MauigRPC/
└── .idea/
    └── .idea.MauigRPC/
        └── .idea/
            └── runConfigurations/
                ├── GrpcJSServiceApi__Dev_.xml   ✅ Shell script
                ├── GrpcJSServiceApi__Prod_.xml  ✅ Shell script
                └── GrpcJSServiceApi__Build_.xml ✅ Shell script
```

Each runs: `cd GrpcJSServiceApi && npm run <script>`

---

## 🎉 Success Checklist

After restarting Rider:

- [ ] ✅ No more "broken configuration" error
- [ ] ✅ Configurations appear in dropdown
- [ ] ✅ Run `npm install` in GrpcJSServiceApi (first time)
- [ ] ✅ Select "GrpcJSServiceApi (Dev)"
- [ ] ✅ Click ▶️ Run
- [ ] ✅ Terminal opens and server starts
- [ ] ✅ See "gRPC Server is running on http://localhost:5195"
- [ ] ✅ Test with grpcurl

---

## 🎊 You're All Set!

The configurations now use **Shell Script** type, which:
- ✅ Works without plugins
- ✅ Runs in integrated terminal
- ✅ Shows all output
- ✅ Easy to stop with ⏹️ button

Just restart Rider and they'll work! 🚀

---

## 📚 Need More Help?

- **npm Scripts panel** - Right-click package.json → Show npm Scripts
- **Manual creation** - See `LOAD_RUN_CONFIGS.md`
- **Terminal method** - Just run `npm run start:dev` in Terminal

Happy coding! 🎉
