# 🚀 Start in Rider - 3 Simple Steps

Follow these exact steps to run the NestJS project in Rider.

---

## ⚡ Method 1: Super Quick (Recommended)

### Step 1: Open the Project
1. Open **Rider**
2. **File** → **Open...**
3. Navigate to: `/Users/jgmlsolucoes/Projetos/POC/MauigRPC/GrpcJSServiceApi`
4. Click **Open**

### Step 2: Install Dependencies
1. **Bottom of Rider** → Click **Terminal** tab (or press `Option+F12`)
2. You should already be in `GrpcJSServiceApi` folder
3. Run:
   ```bash
   npm install
   ```
4. Wait 1-2 minutes (downloading packages)

### Step 3: Run It!
1. **Right-click** on `package.json` in Project panel (left side)
2. Select **Show npm Scripts**
3. A panel appears on the right side
4. **Double-click** on `start:dev`
5. Done! Server starts! 🎉

You should see:
```
🚀 gRPC Server is running on http://localhost:5195
```

---

## 🎯 Method 2: Using Run Configurations (More Control)

I've already created run configurations for you!

### Step 1: Open Project (Same as Above)

### Step 2: Install Dependencies (Same as Above)

### Step 3: Select Run Configuration
1. Look at **top-right corner** of Rider
2. You'll see a dropdown that says **"GrpcJSServiceApi (Dev)"**
3. Click the green **▶️ Run** button next to it
4. Done! Server starts! 🎉

### Available Configurations:
- **GrpcJSServiceApi (Dev)** - Development with hot reload ⭐ Use this one!
- **GrpcJSServiceApi (Prod)** - Production mode
- **GrpcJSServiceApi (Build)** - Build only (compile TypeScript)

---

## 🧪 Test It!

Once the server is running:

### In Rider's Terminal:
```bash
# Test Greeter service
grpcurl -plaintext -d '{"name": "Rider"}' localhost:5195 greet.Greeter/SayHello

# Expected response:
# {
#   "message": "Hello Rider"
# }
```

### Test Contact Service:
```bash
# Get all contacts
grpcurl -plaintext -d '{}' localhost:5195 contact.ContactService/GetAllContacts
```

---

## 🔥 Hot Reload Demo

Try changing the code while it's running!

### Step 1: Make Sure Server is Running
Server should be running in dev mode (`start:dev`)

### Step 2: Open a Service File
1. Open `src/services/greeter.controller.ts`
2. Find line 48: `message: \`Hello ${request.name}\`,`

### Step 3: Change It
Change to:
```typescript
message: `¡Hola ${request.name}! 👋`,
```

### Step 4: Save
Press `Cmd+S` (or `Ctrl+S`)

### Step 5: Watch the Terminal
You'll see:
```
File change detected. Starting incremental compilation...
Found 0 errors. Watching for file changes.
```

### Step 6: Test the Change
```bash
grpcurl -plaintext -d '{"name": "World"}' localhost:5195 greet.Greeter/SayHello
```

Response:
```json
{
  "message": "¡Hola World! 👋"
}
```

**It changed without restarting! 🔥**

---

## 🐛 Debugging in Rider

### Step 1: Set a Breakpoint
1. Open `src/services/greeter.controller.ts`
2. Click in the **gutter** (left margin) next to line 48
3. A red dot appears 🔴

### Step 2: Run in Debug Mode
1. Click the **🐛 Debug** button (next to Run button)
2. Or press `Cmd+D` (Mac) / `Ctrl+D` (Windows)

### Step 3: Trigger the Breakpoint
In terminal, call the service:
```bash
grpcurl -plaintext -d '{"name": "Debug Test"}' localhost:5195 greet.Greeter/SayHello
```

### Step 4: Rider Pauses!
- Execution stops at your breakpoint
- You can inspect `request.name` value
- Use debugger controls:
  - **Step Over** (F8)
  - **Step Into** (F7)
  - **Resume** (F9)

---

## 📊 Useful Rider Panels

### Terminal (Bottom)
- **Access:** `Option+F12` (Mac) or `Alt+F12` (Windows)
- **Use:** Run commands, test with grpcurl

### npm Scripts (Right Side)
- **Access:** Right-click `package.json` → Show npm Scripts
- **Use:** Quick access to all scripts

### Run/Debug Console (Bottom)
- **Auto-opens** when you run the app
- Shows server logs and output

### Project Explorer (Left)
- View all files
- Navigate the project structure

---

## ⚙️ Quick Settings Check

### Verify Node.js is Detected

1. **Rider** → **Preferences** (or `Cmd+,`)
2. **Languages & Frameworks** → **Node.js**
3. You should see:
   - ✅ **Node interpreter:** (path to node)
   - ✅ **Package manager:** npm

If not detected, click **Configure** and select your Node.js installation.

### Check Node.js Version
In Terminal:
```bash
node --version   # Should show v18 or higher
npm --version    # Should show v9 or higher
```

If not installed:
```bash
brew install node
```

---

## 🚨 Common Issues

### Issue: "Cannot find package.json"
**Fix:** Make sure you opened the `GrpcJSServiceApi` folder, not the parent folder.

### Issue: "Node.js interpreter is not configured"
**Fix:**
1. **Preferences** → **Node.js**
2. Click **Configure**
3. Select your Node.js installation
4. If not installed: `brew install node` in terminal

### Issue: "Module not found: @nestjs/core"
**Fix:**
```bash
npm install
```

### Issue: Port 5195 already in use
**Fix:**
```bash
lsof -ti:5195 | xargs kill -9
```

### Issue: Rider doesn't show run configurations
**Fix:** Close and reopen the project. Rider will detect the `.idea/runConfigurations/` folder.

---

## 🎉 Success Checklist

- [x] ✅ Opened project in Rider
- [x] ✅ Ran `npm install`
- [x] ✅ Server running on port 5195
- [x] ✅ Tested with grpcurl
- [x] ✅ Made a code change and saw hot reload
- [x] ✅ Set a breakpoint and debugged

---

## 💡 Pro Tips

### Tip 1: Run Both Services Together
You can run both .NET (5194) and NestJS (5195) at the same time!

1. Run your .NET service first
2. Run NestJS service (this guide)
3. Both appear in **Services** panel at bottom
4. Test both to compare!

### Tip 2: Quick File Navigation
- `Cmd+Shift+O` (Mac) or `Ctrl+Shift+O` (Windows)
- Type filename to jump to it instantly
- Example: Type "greeter" to open `greeter.controller.ts`

### Tip 3: Search Everywhere
- Press `Shift` twice
- Type anything to search across entire project
- Files, classes, methods, symbols, etc.

### Tip 4: Split Editor
- Right-click file tab → **Split Right/Down**
- View proto file and implementation side-by-side!

### Tip 5: npm Scripts Panel Always Visible
- Right-click `package.json` → **Show npm Scripts**
- Pin the panel by clicking the pin icon
- Quick access to all scripts

---

## 📚 What to Read Next

Now that it's running:

1. ✅ **You're here!** Running in Rider
2. 📖 **UNDERSTANDING_NESTJS.md** - Learn how it works
3. 📖 **COMPARISON.md** - See .NET vs NestJS differences
4. 🛠️ **Make changes** - Try modifying services

---

## 🆘 Still Stuck?

### Check These:
1. ✅ Node.js installed? (`node --version`)
2. ✅ In correct folder? (Should be `GrpcJSServiceApi`)
3. ✅ Ran `npm install`?
4. ✅ No other service on port 5195?

### Get Help:
- Read **RIDER_SETUP.md** for detailed Rider guide
- Read **TROUBLESHOOTING** section in that file
- Check Rider logs: **Help** → **Show Log in Finder**

---

## 🎊 You Did It!

Your NestJS gRPC service is running in Rider!

**Now you can:**
- ✏️ Edit TypeScript code with IntelliSense
- 🐛 Debug with breakpoints
- 🔥 See changes instantly with hot reload
- 🧪 Test from Rider's terminal
- 🚀 Build something awesome!

Happy coding! 🎉
