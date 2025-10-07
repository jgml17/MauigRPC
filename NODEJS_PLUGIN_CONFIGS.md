# ✅ Updated Run Configurations (Node.js Plugin Enabled!)

All configurations have been updated to use the **npm type** with full debugging support!

---

## 🎉 What Changed?

Now that you have the Node.js plugin installed, I've converted all configurations from **Shell Script** type to **npm** type.

### Before (Shell Script):
- ❌ No debugging support
- ❌ Ran in terminal only
- ✅ Worked without plugins

### After (npm with Node.js plugin):
- ✅ **Full debugging support!** 🐛
- ✅ Better integration with Rider
- ✅ Click 🐛 button to debug
- ✅ Click ▶️ button to run

---

## 🚀 Available Configurations

After restarting Rider, you'll see:

### 1. **GrpcJSServiceApi (Dev)** ⭐
- **Script:** `npm run start:dev`
- **Features:** Hot reload, no debugging
- **Use:** Regular development
- **Run:** Click ▶️
- **Can't debug:** This one doesn't have `--debug` flag

### 2. **GrpcJSServiceApi (Debug)** 🐛 ⭐ NEW!
- **Script:** `npm run start:debug`
- **Features:** Hot reload + debugging
- **Use:** When you need to debug
- **Run:** Click ▶️ (runs with debugger attached)
- **Debug:** Click 🐛 (runs with debugger attached + pauses on breakpoints)

### 3. **GrpcJSServiceApi (Prod)**
- **Script:** `npm run start:prod`
- **Features:** Production mode
- **Use:** Testing production build
- **Must build first:** Run "Build" config first

### 4. **GrpcJSServiceApi (Build)**
- **Script:** `npm run build`
- **Features:** Compiles TypeScript to JavaScript
- **Use:** Before running Prod

---

## 🐛 How to Debug Now

### Method 1: Use the Debug Configuration (Easiest!) ⭐

1. **Select configuration:** `GrpcJSServiceApi (Debug)` from dropdown
2. **Set breakpoints:** Click in left margin (gutter) of any `.ts` file
3. **Click 🐛 Debug button** (or press `Cmd+D` / `Ctrl+D`)
4. **Service starts with debugger attached**
5. **Call the service:**
   ```bash
   grpcurl -plaintext -d '{"name": "Test"}' localhost:5195 greet.Greeter/SayHello
   ```
6. **Rider pauses at breakpoints!** 🎉

### Method 2: Debug the Dev Configuration

The Dev configuration doesn't have `--debug` flag, but you can still debug:

1. **Run Dev config normally** (click ▶️)
2. **Add breakpoints**
3. **Run** → **Attach to Process...** → Select node process
4. Works but less convenient than using Debug config

---

## 📊 Configuration Comparison

| Configuration | Hot Reload | Debugger | Best For |
|--------------|-----------|----------|----------|
| **Dev** | ✅ | ❌ | Regular coding |
| **Debug** 🐛 | ✅ | ✅ | **Debugging** ⭐ |
| **Prod** | ❌ | ❌ | Production test |
| **Build** | N/A | N/A | Compiling |

---

## 🎯 Quick Start

### Step 1: Restart Rider
Close and reopen Rider for configurations to reload.

### Step 2: Check Dropdown
You should see all 4 configurations (including the new **Debug** one!)

### Step 3: Try Debugging!

**Quick test:**
1. Select **"GrpcJSServiceApi (Debug)"**
2. Open `src/services/greeter.controller.ts`
3. Set breakpoint on line 48 (click in left margin)
4. Click **🐛 Debug** button
5. Wait for server to start
6. In Terminal:
   ```bash
   grpcurl -plaintext -d '{"name": "Debug Test"}' localhost:5195 greet.Greeter/SayHello
   ```
7. **Rider pauses at breakpoint!** 🎉

---

## 🎨 Visual Guide

### What You'll See in Rider:

```
┌─────────────────────────────────────────┐
│ Top-right dropdown:                     │
│ ▼ GrpcJSServiceApi (Debug)      ▶️ 🐛  │
└─────────────────────────────────────────┘

Click dropdown to see all:
▼ Run Configurations
  ├── GrpcJSServiceApi (Dev)    → ▶️ only
  ├── GrpcJSServiceApi (Debug)  → ▶️ and 🐛 ⭐
  ├── GrpcJSServiceApi (Prod)   → ▶️ only
  └── GrpcJSServiceApi (Build)  → ▶️ only
```

### When Debugging:

```
┌──────────────────────────────────────────┐
│ Debugger Panel (bottom):                 │
├──────────────────────────────────────────┤
│ Variables:                               │
│   request = {name: "Debug Test"}         │
│   this = GreeterController {...}         │
├──────────────────────────────────────────┤
│ Call Stack:                              │
│   sayHello (greeter.controller.ts:48)    │
│   [external code]                        │
├──────────────────────────────────────────┤
│ Console:                                 │
│   [Nest] Starting Nest application...   │
│   [Nest] Debugger attached              │
└──────────────────────────────────────────┘
```

---

## 💡 Pro Tips

### Tip 1: Keyboard Shortcuts
- **Run:** `Cmd+R` (Mac) / `Ctrl+R` (Windows)
- **Debug:** `Cmd+D` (Mac) / `Ctrl+D` (Windows)
- **Stop:** `Cmd+F2` (Mac) / `Ctrl+F2` (Windows)

### Tip 2: Hot Reload While Debugging
- Debug config has `--watch` flag
- Change code → Save → Auto-recompiles
- Debugger stays attached!

### Tip 3: Conditional Breakpoints
- Right-click breakpoint → **Edit Breakpoint...**
- Add condition: `request.name === "Test"`
- Only breaks when condition is true!

### Tip 4: Evaluate Expressions
- While paused at breakpoint
- Press `Option+F8` (Mac) / `Alt+F8` (Windows)
- Type any expression: `request.name.toUpperCase()`
- See result immediately!

### Tip 5: Debug Both Services
- Run .NET service in debug mode
- Run NestJS Debug configuration
- Both debuggable simultaneously!

---

## 🔄 Switching Between Configurations

### For Regular Development:
Use **"GrpcJSServiceApi (Dev)"**
- Faster startup (no debugger overhead)
- Hot reload works
- Click ▶️ to run

### When You Need to Debug:
Use **"GrpcJSServiceApi (Debug)"**
- Slightly slower startup (debugger enabled)
- Hot reload works
- Click 🐛 to debug

### Testing Production Build:
1. First run: **"GrpcJSServiceApi (Build)"**
2. Then run: **"GrpcJSServiceApi (Prod)"**

---

## 🚨 Troubleshooting

### Configuration shows as "broken"

**Fix:** Make sure Node.js plugin is enabled:
1. **Rider** → **Preferences**
2. **Plugins**
3. Search for "Node.js"
4. Make sure it's ✅ enabled

### "Cannot find package.json"

**Fix:** Configurations point to wrong location. Check:
- Path should be: `$PROJECT_DIR$/GrpcJSServiceApi/package.json`
- Edit configuration and browse to correct file

### Debugger not stopping at breakpoints

**Check:**
1. ✅ Using **Debug** configuration (not Dev)
2. ✅ Clicked **🐛 Debug** button (not ▶️ Run)
3. ✅ Breakpoint is on executable line
4. ✅ Source maps enabled in `tsconfig.json`

### "Node interpreter not configured"

**Fix:**
1. **Rider** → **Preferences**
2. **Languages & Frameworks** → **Node.js**
3. Select Node.js interpreter
4. Click **OK**

---

## 🎉 Success Checklist

After restarting Rider:

- [ ] ✅ See 4 configurations in dropdown
- [ ] ✅ "GrpcJSServiceApi (Debug)" configuration exists
- [ ] ✅ Select Debug config
- [ ] ✅ Set breakpoint in greeter.controller.ts
- [ ] ✅ Click 🐛 Debug button
- [ ] ✅ Server starts
- [ ] ✅ Call service with grpcurl
- [ ] ✅ Rider pauses at breakpoint!
- [ ] ✅ Can inspect variables
- [ ] ✅ Can step through code (F8, F7, F9)

---

## 📚 Summary

### What You Now Have:

1. ✅ **Dev** config - Regular development with hot reload
2. ✅ **Debug** config - Development with debugging 🐛⭐
3. ✅ **Prod** config - Production testing
4. ✅ **Build** config - Compile only

### How to Debug:

**Quick:** Select "Debug" → Click 🐛 → Set breakpoints → Done!

**That's it!** Much simpler than the "Attach to Process" method!

---

## 🎊 Enjoy Your New Debugging Setup!

Now you can:
- ✅ Click 🐛 to debug instantly
- ✅ Set breakpoints in TypeScript
- ✅ Inspect variables
- ✅ Step through code
- ✅ Evaluate expressions
- ✅ Debug with hot reload

Just select **"GrpcJSServiceApi (Debug)"** and click 🐛!

Happy debugging! 🚀
