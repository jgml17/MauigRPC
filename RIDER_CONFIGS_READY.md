# ✅ Rider Run Configurations Are Now Ready!

The run configurations have been installed in your solution!

---

## 🎯 How to Load Them

### Step 1: Close Rider (if open)
If Rider is currently open with the MauigRPC solution, close it completely.

### Step 2: Reopen Your Solution
1. Open **Rider**
2. **File** → **Open Recent** → **MauigRPC.sln**
   
   Or:
   
   **File** → **Open...** → Select `/Users/jgmlsolucoes/Projetos/POC/MauigRPC/MauigRPC.sln`

### Step 3: Look at Top-Right Corner
Check the **run configuration dropdown** (top-right corner).

You should now see:
```
▼ Run Configurations
  ├── [Your existing .NET configurations]
  ├── GrpcJSServiceApi (Dev)   ⭐ NEW!
  ├── GrpcJSServiceApi (Prod)  ⭐ NEW!
  └── GrpcJSServiceApi (Build) ⭐ NEW!
```

---

## 🚀 Quick Test

### 1. Select Configuration
Click the dropdown → Select **"GrpcJSServiceApi (Dev)"**

### 2. Install Dependencies First
Open **Terminal** in Rider (`Option+F12`):
```bash
cd GrpcJSServiceApi
npm install
```
Wait for it to complete (1-2 minutes).

### 3. Run It!
Click the **▶️ Run** button (or press `Cmd+R`)

### 4. Success!
You should see:
```
🚀 gRPC Server is running on http://localhost:5195

Available Services:
  - greet.Greeter
  - contact.ContactService
```

---

## 📊 What Each Configuration Does

### GrpcJSServiceApi (Dev) ⭐ Recommended
- Runs with **hot reload**
- Automatically restarts on code changes
- Perfect for development
- **Script:** `npm run start:dev`

### GrpcJSServiceApi (Prod)
- Runs in production mode
- No hot reload
- Optimized for performance
- Must build first: `npm run build`
- **Script:** `npm run start:prod`

### GrpcJSServiceApi (Build)
- Only compiles TypeScript to JavaScript
- Doesn't run the server
- Output goes to `dist/` folder
- **Script:** `npm run build`

---

## 🔧 Configuration Details

The configurations are stored here:
```
MauigRPC/
└── .idea/
    └── .idea.MauigRPC/
        └── .idea/
            └── runConfigurations/
                ├── GrpcJSServiceApi__Dev_.xml   ✅
                ├── GrpcJSServiceApi__Prod_.xml  ✅
                └── GrpcJSServiceApi__Build_.xml ✅
```

All point to:
```
$PROJECT_DIR$/GrpcJSServiceApi/package.json
```

Which Rider resolves as:
```
/Users/jgmlsolucoes/Projetos/POC/MauigRPC/GrpcJSServiceApi/package.json
```

---

## 🎮 Using the Configurations

### From Top-Right Dropdown:
1. **Click dropdown** → Select configuration
2. **Click ▶️** to run
3. **Click 🐛** to debug
4. **Click ⏹️** to stop

### Keyboard Shortcuts:
- **Run:** `Cmd+R` (Mac) / `Ctrl+R` (Windows)
- **Debug:** `Cmd+D` (Mac) / `Ctrl+D` (Windows)
- **Stop:** `Cmd+F2` (Mac) / `Ctrl+F2` (Windows)

---

## 🔥 Create Compound Configuration (Run Both!)

Want to run both .NET and NestJS together?

### Steps:
1. **Run** → **Edit Configurations...**
2. Click **+** → **Compound**
3. **Name:** `All Services`
4. Click **+** in right panel, add:
   - Your .NET gRPC configuration
   - `GrpcJSServiceApi (Dev)`
5. Check **"Run configurations in parallel"**
6. Click **OK**

Now select **"All Services"** and run both at once!
- .NET on port **5194**
- NestJS on port **5195**

---

## 🐛 If Configurations Don't Appear

### Try This:
1. **File** → **Invalidate Caches...**
2. Select **"Invalidate and Restart"**
3. Wait for Rider to restart
4. Configurations should appear

### Or Manually Verify:
Check the run configuration dropdown shows them. If not:
1. **Run** → **Edit Configurations...**
2. They should be listed on the left
3. If not, follow the manual creation steps in `LOAD_RUN_CONFIGS.md`

---

## 🎉 Success Checklist

After reopening Rider:

- [ ] ✅ See configurations in dropdown
- [ ] ✅ Run `npm install` in GrpcJSServiceApi folder
- [ ] ✅ Select "GrpcJSServiceApi (Dev)"
- [ ] ✅ Click ▶️ Run
- [ ] ✅ See "gRPC Server is running on http://localhost:5195"
- [ ] ✅ Test with grpcurl

---

## 🧪 Test Both Services

With both services running:

```bash
# Terminal in Rider
# Test .NET service (5194)
grpcurl -plaintext -d '{"name": "World"}' localhost:5194 greet.Greeter/SayHello

# Test NestJS service (5195)
grpcurl -plaintext -d '{"name": "World"}' localhost:5195 greet.Greeter/SayHello
```

Both should respond with:
```json
{
  "message": "Hello World"
}
```

---

## 📚 Related Documentation

- **START_IN_RIDER.md** - Complete Rider setup guide
- **LOAD_RUN_CONFIGS.md** - Detailed configuration instructions
- **ADD_TO_SOLUTION.md** - Adding project to solution
- **QUICKSTART.md** - General quickstart guide

---

## 🎊 You're All Set!

The run configurations are now in the correct location and Rider will load them automatically when you reopen the solution.

Just:
1. **Close Rider** (if open)
2. **Reopen MauigRPC.sln**
3. **Look for the configurations** in the dropdown
4. **Run them!** 🚀

Happy coding! 🎉
