# Running NestJS gRPC Service in JetBrains Rider

Complete guide to run and debug the NestJS project in Rider IDE.

---

## 🚀 Quick Setup (3 Steps)

### Step 1: Install Dependencies

Open Rider's **Terminal** (bottom of IDE) and run:

```bash
cd GrpcJSServiceApi
npm install
```

Wait for packages to download (might take 1-2 minutes).

### Step 2: Create Run Configuration

**Option A: Let Rider Auto-Detect (Easiest)**

1. Open `GrpcJSServiceApi` folder in Rider
2. Right-click on `package.json`
3. Select **"Show npm Scripts"**
4. Double-click `start:dev` to run

**Option B: Manual Configuration (Recommended)**

1. Click **Run** → **Edit Configurations...**
2. Click **+** (Add New Configuration)
3. Select **npm**
4. Configure:
   - **Name:** `GrpcJSServiceApi (Dev)`
   - **Package.json:** `/Users/jgmlsolucoes/Projetos/POC/MauigRPC/GrpcJSServiceApi/package.json`
   - **Command:** `run`
   - **Scripts:** `start:dev`
5. Click **OK**

### Step 3: Run It!

Click the **▶️ Run** button (or press `Ctrl+R` / `Cmd+R`)

You should see:
```
🚀 gRPC Server is running on http://localhost:5195
```

---

## 📁 Opening the Project in Rider

### Method 1: Open as Separate Project
1. **File** → **Open**
2. Navigate to `/Users/jgmlsolucoes/Projetos/POC/MauigRPC/GrpcJSServiceApi`
3. Click **Open**

### Method 2: Add to Existing Solution
If you have the .NET solution open:
1. Right-click solution in **Solution Explorer**
2. **Add** → **Existing Project**
3. Select `GrpcJSServiceApi` folder

---

## 🎨 Rider Configuration Files

Rider stores configurations in `.idea/` folder. I'll create the run configuration for you.

---

## 🔧 Run Configurations

### Development Mode (Hot Reload)
```
Name: GrpcJSServiceApi (Dev)
Type: npm
Script: start:dev
```
This automatically restarts when you change files.

### Production Mode
```
Name: GrpcJSServiceApi (Prod)
Type: npm
Script: start:prod
```
Run this after building (`npm run build`).

### Build Only
```
Name: GrpcJSServiceApi (Build)
Type: npm
Script: build
```
Compiles TypeScript to JavaScript.

---

## 🐛 Debugging in Rider

### Enable Debugging

1. **Run** → **Edit Configurations...**
2. Select your npm configuration
3. Check **"Enable debug"** (if available)
4. Or create a **Node.js** configuration:
   - **Name:** `GrpcJSServiceApi (Debug)`
   - **Working directory:** `/Users/jgmlsolucoes/Projetos/POC/MauigRPC/GrpcJSServiceApi`
   - **JavaScript file:** `dist/main.js`
   - **Application parameters:** (leave empty)

### Set Breakpoints

1. Open `src/services/greeter.controller.ts`
2. Click in the gutter (left margin) next to line 48: `return { message: ... }`
3. A red dot appears
4. Run in **Debug mode** (🐛 icon)
5. Call the service with grpcurl
6. Rider will pause at your breakpoint!

---

## 📊 Useful Rider Panels

### 1. Terminal (Bottom)
- Run commands like `npm install`, `npm run start:dev`
- Access: **View** → **Tool Windows** → **Terminal**

### 2. npm Scripts (Right Side)
- View all available scripts from `package.json`
- Double-click to run any script
- Access: Right-click `package.json` → **Show npm Scripts**

### 3. Structure (Left Side)
- See all classes, methods, and decorators
- Access: **View** → **Tool Windows** → **Structure**

### 4. Services (Bottom)
- Monitor running Node.js processes
- Access: **View** → **Tool Windows** → **Services**

---

## ⚙️ Rider Settings for Better Experience

### Enable Node.js Support

1. **Rider** → **Preferences** (or `Cmd+,`)
2. **Languages & Frameworks** → **Node.js**
3. Set **Node interpreter:** (should auto-detect)
   - If not: `/usr/local/bin/node` or wherever `node` is installed
4. Click **OK**

### Enable TypeScript Support

1. **Rider** → **Preferences**
2. **Languages & Frameworks** → **TypeScript**
3. Check **"TypeScript Language Service"**
4. **TypeScript version:** Should show from `node_modules`

### Code Style

1. **Rider** → **Preferences**
2. **Editor** → **Code Style** → **TypeScript**
3. Rider will auto-format on save if configured

---

## 🎯 Quick Actions in Rider

| Action | Shortcut (Mac) | Shortcut (Windows/Linux) |
|--------|----------------|--------------------------|
| Run | `Cmd+R` | `Ctrl+R` |
| Debug | `Cmd+D` | `Ctrl+D` |
| Stop | `Cmd+F2` | `Ctrl+F2` |
| Open Terminal | `Option+F12` | `Alt+F12` |
| Find in Files | `Cmd+Shift+F` | `Ctrl+Shift+F` |
| Go to File | `Cmd+Shift+N` | `Ctrl+Shift+N` |
| Navigate to Symbol | `Cmd+Option+Shift+N` | `Ctrl+Alt+Shift+N` |

---

## 🔍 Testing from Rider

### Using Rider's Terminal

1. Open **Terminal** in Rider
2. Make sure you're in `GrpcJSServiceApi` folder
3. Run test commands:

```bash
# Test Greeter service
grpcurl -plaintext -d '{"name": "Rider"}' localhost:5195 greet.Greeter/SayHello

# Get all contacts
grpcurl -plaintext -d '{}' localhost:5195 contact.ContactService/GetAllContacts

# Get contact by ID
grpcurl -plaintext -d '{"id": 1}' localhost:5195 contact.ContactService/GetContact
```

### Using HTTP Client (for REST APIs)
For gRPC, you'll need grpcurl or Postman. Rider's HTTP Client doesn't support gRPC directly.

---

## 📝 Creating npm Run Configurations

### Visual Guide:

1. **Top-right corner** → Click dropdown (says "Add Configuration...")
2. Click **Edit Configurations...**
3. Click **+** → **npm**
4. Fill in:
   ```
   Name: GrpcJSServiceApi (Dev)
   package.json: [Browse to package.json]
   Command: run
   Scripts: start:dev
   ```
5. **Apply** → **OK**

Now you can:
- Click **▶️** to run
- Click **🐛** to debug
- Click **⏹️** to stop

---

## 🚨 Troubleshooting

### "Node.js interpreter is not configured"

**Fix:**
1. **Rider** → **Preferences**
2. **Languages & Frameworks** → **Node.js**
3. Click **Configure** → Select Node.js installation
4. If not installed: `brew install node`

### "Cannot find module '@nestjs/core'"

**Fix:**
```bash
cd GrpcJSServiceApi
npm install
```

### Port 5195 already in use

**Fix in Rider Terminal:**
```bash
lsof -ti:5195 | xargs kill -9
```

Or change port in `src/main.ts`:
```typescript
url: '0.0.0.0:5196',  // Use different port
```

### TypeScript errors in Rider

**Fix:**
1. Right-click `package.json`
2. **npm** → **Install**
3. Wait for completion
4. **File** → **Invalidate Caches** → **Invalidate and Restart**

---

## 🎨 Making Code Changes in Rider

### Example: Change Greeting Message

1. Open `src/services/greeter.controller.ts`
2. Find line 48: `message: \`Hello ${request.name}\``
3. Change to: `message: \`Olá ${request.name}!\``
4. Save file (`Cmd+S`)
5. If running in dev mode, it auto-reloads!
6. Test: `grpcurl -plaintext -d '{"name": "World"}' localhost:5195 greet.Greeter/SayHello`

### Hot Reload Works! 🔥

When running `npm run start:dev`, Rider automatically:
- Detects file changes
- Recompiles TypeScript
- Restarts the server
- No need to manually restart!

---

## 📦 Project Structure in Rider

```
GrpcJSServiceApi/
├── 📁 src/                      (TypeScript source)
│   ├── main.ts                 (Entry point - has ▶️ icon)
│   ├── app.module.ts           (Module configuration)
│   ├── 📁 protos/              (Protocol buffers)
│   └── 📁 services/            (Service implementations)
├── 📁 node_modules/            (Auto-generated - hide this)
├── 📁 dist/                    (Compiled JS - auto-generated)
├── package.json                (Dependencies - has npm icon)
├── tsconfig.json               (TypeScript config)
└── Documentation files         (*.md files)
```

### Hiding Unnecessary Folders

1. Right-click `node_modules` folder
2. **Mark Directory as** → **Excluded**
3. Repeat for `dist/` folder

This keeps Rider fast and focused on your source code.

---

## 🔄 Running Both .NET and NestJS Together

You can run both services simultaneously in Rider:

### Option 1: Separate Run Configurations
1. **Run** → **Edit Configurations...**
2. Create **Compound** configuration:
   - **Name:** `All Services`
   - Add: Your .NET gRPC service (5194)
   - Add: Your NestJS service (5195)
3. Click **OK**
4. Run the compound configuration

### Option 2: Multiple Run Tabs
1. Run .NET service first (port 5194)
2. Run NestJS service in new tab (port 5195)
3. Both appear in **Services** panel

---

## 💡 Pro Tips for Rider

### 1. Quick Switch Between .NET and Node.js
- `Cmd+E` → Recent Files
- Switch between C# and TypeScript files instantly

### 2. Find Usages
- Right-click any method/class
- **Find Usages** (`Option+F7`)
- See where it's called

### 3. Refactor/Rename
- Right-click method name
- **Refactor** → **Rename** (`Shift+F6`)
- Updates everywhere!

### 4. Auto-Import
- Type `@Controller()` 
- Rider auto-suggests import from `@nestjs/common`
- Press `Tab` to auto-import

### 5. Format Code
- `Cmd+Option+L` - Format entire file
- Rider respects TypeScript formatting rules

---

## 🎉 You're Ready!

### Quick Start Checklist:

- [ ] Open project in Rider
- [ ] Open Terminal: `cd GrpcJSServiceApi`
- [ ] Install: `npm install`
- [ ] Create npm run configuration (start:dev)
- [ ] Click ▶️ Run button
- [ ] See "gRPC Server is running on http://localhost:5195"
- [ ] Test with grpcurl

---

## 📚 Next Steps

1. ✅ Get it running in Rider
2. 🐛 Try debugging with breakpoints
3. ✏️ Make a code change and see hot reload
4. 🧪 Test with grpcurl in Rider terminal
5. 🚀 Build something new!

---

## 🆘 Need Help?

- **Rider Help:** Help → Rider Help
- **Node.js Plugin:** Already built-in
- **Terminal:** Access via `Option+F12`

Happy coding in Rider! 🎉
