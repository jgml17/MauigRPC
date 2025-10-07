# 🐛 Debugging NestJS in Rider

Complete guide to debug your NestJS gRPC service with breakpoints in Rider.

---

## 🎯 Two Methods to Debug

### Method 1: Attach to Running Process (Easiest) ⭐
Debug a service that's already running

### Method 2: Node.js Debug Configuration
Start the service in debug mode from Rider

---

## ⭐ Method 1: Attach to Running Process (Recommended)

This is the easiest way to debug your NestJS app in Rider.

### Step 1: Start Service in Debug Mode

In Rider's Terminal:
```bash
cd GrpcJSServiceApi
npm run start:debug
```

You'll see:
```
Debugger listening on ws://127.0.0.1:9229/...
For help, see: https://nodejs.org/en/docs/inspector
Watching for file changes...
```

### Step 2: Set Breakpoints

1. Open `src/services/greeter.controller.ts`
2. Click in the **gutter** (left margin) next to line 48
3. A red dot appears 🔴

### Step 3: Attach Debugger

1. **Run** → **Attach to Process...** (or `Cmd+Option+Shift+P`)
2. Search for **"node"**
3. You'll see: `node ... start:debug`
4. Click it to attach

### Step 4: Trigger Breakpoint

In another terminal:
```bash
grpcurl -plaintext -d '{"name": "Debug Test"}' localhost:5195 greet.Greeter/SayHello
```

### Step 5: Rider Pauses! 🎉

- Execution stops at your breakpoint
- Inspect variables: `request.name`
- Step through code:
  - **F8** - Step Over
  - **F7** - Step Into
  - **F9** - Resume Program

---

## 🔧 Method 2: Node.js Debug Configuration

Create a dedicated debug configuration.

### Step 1: Add Debug Script

The debug script already exists in `package.json`:
```json
"start:debug": "nest start --debug --watch"
```

### Step 2: Create Run Configuration

1. **Run** → **Edit Configurations...**
2. Click **+** → **Shell Script**
3. Fill in:
   - **Name:** `GrpcJSServiceApi (Start Debug Mode)`
   - **Execute:** `Script text`
   - **Script text:** `cd $PROJECT_DIR$/GrpcJSServiceApi && npm run start:debug`
   - **Working directory:** `$PROJECT_DIR$/GrpcJSServiceApi`
   - **Execute in terminal:** ✅ Check this
4. Click **OK**

### Step 3: Run in Debug Mode

1. Select the configuration
2. Click **▶️ Run** (not Debug button)
3. Service starts with debugger enabled

### Step 4: Attach Debugger

1. **Run** → **Attach to Process...**
2. Find the node process
3. Attach to it

### Step 5: Set Breakpoints & Test

Same as Method 1!

---

## 🎨 Visual Guide: Debugging Flow

```
1. Start service with debugger:
   npm run start:debug
   ↓
2. Service runs, listening on port 9229
   Debugger listening on ws://127.0.0.1:9229
   ↓
3. Set breakpoints in Rider
   🔴 Click in gutter next to code
   ↓
4. Attach debugger in Rider
   Run → Attach to Process → Select node
   ↓
5. Call the service
   grpcurl -plaintext -d '{"name": "Test"}' ...
   ↓
6. Rider pauses at breakpoint! 🎉
   - Inspect variables
   - Step through code
   - Evaluate expressions
```

---

## 🔍 Debug Features Available

### Inspect Variables
- Hover over any variable
- See values in **Variables** panel
- Expand objects to see properties

### Call Stack
- See the full call stack
- Navigate between stack frames
- See where you are in the execution

### Evaluate Expressions
- **Alt+F8** - Evaluate Expression
- Type any JavaScript/TypeScript expression
- See the result immediately

### Console
- See all console.log output
- Interact with the running process

### Watch Variables
- Add variables to watch list
- Track values as you step through

---

## 📋 package.json Scripts

Your project has these npm scripts:

```json
{
  "start": "nest start",
  "start:dev": "nest start --watch",          // Hot reload, no debug
  "start:debug": "nest start --debug --watch", // Hot reload + debug ⭐
  "start:prod": "node dist/main"              // Production
}
```

### Which to Use?

| Script | Hot Reload | Debug | Use When |
|--------|-----------|-------|----------|
| `start:dev` | ✅ | ❌ | Regular development |
| `start:debug` | ✅ | ✅ | **Debugging** ⭐ |
| `start:prod` | ❌ | ❌ | Production |

---

## 🎯 Quick Debug Session

### Complete Example:

```bash
# 1. Start in debug mode (Terminal 1)
cd GrpcJSServiceApi
npm run start:debug

# Output:
# Debugger listening on ws://127.0.0.1:9229/...
# 🚀 gRPC Server is running on http://localhost:5195
```

**In Rider:**
1. Open `src/services/greeter.controller.ts`
2. Set breakpoint on line 48 (the return statement)
3. **Run** → **Attach to Process...**
4. Select the node process
5. Click **Attach**

```bash
# 2. Test the service (Terminal 2)
grpcurl -plaintext -d '{"name": "Debug"}' localhost:5195 greet.Greeter/SayHello
```

**Rider pauses at breakpoint!**
- Inspect `request.name` → Shows "Debug"
- Press **F9** to continue
- Service responds to grpcurl

---

## 🔥 Debugging Contact Service

### Example: Debug CreateContact

1. **Start in debug mode:**
   ```bash
   npm run start:debug
   ```

2. **Set breakpoint** in `contact.controller.ts` line 195:
   ```typescript
   createContact(request: CreateContactRequest): ContactReply {
     // Set breakpoint here ↓
     if (!request.name || request.name.trim() === '') {
   ```

3. **Attach debugger** in Rider

4. **Call the service:**
   ```bash
   grpcurl -plaintext -d '{
     "name": "Test User",
     "address": {"street": "123 Main", "city": "SF", "state": "CA", "zip_code": "94102", "country": "USA"},
     "phone_numbers": [{"number": "+1-555-0000", "type": "MOBILE"}]
   }' localhost:5195 contact.ContactService/CreateContact
   ```

5. **Debug!**
   - Inspect `request.name`
   - Step through validation
   - See contact creation
   - Watch it being added to Map

---

## ⚙️ Advanced: Configure Debug Port

By default, Node.js debugger uses port **9229**.

### To Use Different Port:

Edit `package.json`:
```json
"start:debug": "nest start --debug=9230 --watch"
```

Then attach to that port.

---

## 🚨 Troubleshooting

### Issue: "Cannot attach to process"

**Fix:** Make sure service is running with `--debug` flag:
```bash
npm run start:debug
```

Look for:
```
Debugger listening on ws://127.0.0.1:9229/...
```

### Issue: Breakpoints are greyed out

**Fix:** 
1. Make sure source maps are enabled (they are by default)
2. Check `tsconfig.json` has: `"sourceMap": true`
3. Rebuild: `npm run build`

### Issue: "Cannot find node process"

**Fix:** Make sure node is in PATH:
```bash
which node
# Should show: /usr/local/bin/node or similar
```

### Issue: Breakpoint not hit

**Fix:**
1. Make sure you attached the debugger
2. Verify the service is running
3. Check you're calling the right endpoint
4. Try setting breakpoint on first line of method

---

## 💡 Pro Tips

### Tip 1: Conditional Breakpoints
- Right-click breakpoint
- **Edit Breakpoint...**
- Add condition: `request.name === "Debug"`
- Breakpoint only triggers when condition is true!

### Tip 2: Log Breakpoints
- Right-click breakpoint
- Check **"Evaluate and log"**
- Enter expression: `"Request name: " + request.name`
- Logs to console without stopping!

### Tip 3: Hot Reload While Debugging
- `start:debug` has `--watch`
- Change code → saves → auto-recompiles
- Debugger stays attached!
- No need to restart!

### Tip 4: Debug Multiple Services
- Start .NET service in debug mode
- Start NestJS service in debug mode
- Attach to both
- Debug both simultaneously!

### Tip 5: Use Debug Console
- While paused at breakpoint
- **View** → **Tool Windows** → **Debug**
- **Console** tab
- Execute JavaScript: `request.name.toUpperCase()`

---

## 🎉 Success Checklist

- [ ] ✅ Run `npm run start:debug`
- [ ] ✅ See "Debugger listening on ws://127.0.0.1:9229"
- [ ] ✅ Set breakpoint (red dot appears)
- [ ] ✅ **Run** → **Attach to Process**
- [ ] ✅ Find and attach to node process
- [ ] ✅ Call service with grpcurl
- [ ] ✅ Rider pauses at breakpoint!
- [ ] ✅ Inspect variables
- [ ] ✅ Step through code (F8, F7, F9)
- [ ] ✅ Resume execution (F9)

---

## 📚 Keyboard Shortcuts

| Action | Mac | Windows/Linux |
|--------|-----|---------------|
| **Attach to Process** | `Cmd+Option+Shift+P` | `Ctrl+Alt+Shift+P` |
| **Toggle Breakpoint** | `Cmd+F8` | `Ctrl+F8` |
| **Step Over** | `F8` | `F8` |
| **Step Into** | `F7` | `F7` |
| **Resume Program** | `F9` | `F9` |
| **Evaluate Expression** | `Option+F8` | `Alt+F8` |
| **View Breakpoints** | `Cmd+Shift+F8` | `Ctrl+Shift+F8` |

---

## 🎊 You're Ready to Debug!

Now you can:
- ✅ Set breakpoints in TypeScript
- ✅ Attach debugger from Rider
- ✅ Inspect variables
- ✅ Step through code
- ✅ Fix bugs faster!

**Just run `npm run start:debug` and attach!** 🚀

Happy debugging! 🐛➡️✨
