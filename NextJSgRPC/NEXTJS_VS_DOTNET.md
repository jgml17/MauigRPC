# Next.js vs .NET MAUI: Complete Comparison

A detailed comparison for .NET developers learning Next.js

---

## 🎯 Big Picture

### Your MAUI App (C#)
- **Desktop/Mobile** app
- Calls gRPC **directly** from C# code
- UI built with **XAML**
- Runs on **user's device**

### Next.js App (TypeScript)
- **Web** app
- Browser → API Route → gRPC Client
- UI built with **React/JSX**
- Runs in **browser** (client) + **Node.js** (server)

---

## 📁 Project Structure Comparison

### .NET MAUI Project
```
MauigRPC/
├── MainPage.xaml           ← UI markup
├── MainPage.xaml.cs        ← Code-behind
├── MauiProgram.cs          ← App configuration
└── Platforms/              ← Platform-specific code
```

### Next.js Project
```
NextJSgRPC/
├── src/
│   ├── app/
│   │   ├── page.tsx        ← Home page (UI + logic combined)
│   │   ├── layout.tsx      ← App layout
│   │   └── api/            ← API routes (server-side)
│   │       ├── greet/route.ts
│   │       └── contacts/route.ts
│   ├── lib/
│   │   └── grpc-clients.ts ← gRPC client library
│   └── components/         ← Reusable UI components
├── proto/                  ← Proto files
└── package.json            ← Dependencies (like .csproj)
```

---

## 🔧 Key Concepts Comparison

### 1. Project Configuration

| Concept | .NET MAUI | Next.js |
|---------|-----------|---------|
| **Project file** | `.csproj` | `package.json` |
| **Dependencies** | NuGet packages | npm packages |
| **Entry point** | `MauiProgram.cs` | `layout.tsx` |
| **Configuration** | `appsettings.json` | `next.config.js` |

### 2. UI Framework

| Concept | .NET MAUI | Next.js |
|---------|-----------|---------|
| **UI Markup** | XAML | JSX/TSX (React) |
| **Code-behind** | `.xaml.cs` | Same file as UI |
| **Components** | Controls (Button, Entry, Label) | React Components |
| **Styling** | CSS/Styles in XAML | Tailwind CSS / CSS |

### 3. gRPC Client

| Concept | .NET MAUI | Next.js |
|---------|-----------|---------|
| **Where it runs** | Desktop/Mobile app | Server (API routes) |
| **Client creation** | `new GreeterClient(channel)` | `new GreeterClient(address)` |
| **Call method** | `await client.SayHelloAsync()` | `await client.sayHello()` |
| **Proto loading** | Auto-generated at build time | Dynamic loading with @grpc/proto-loader |

---

## 💻 Code Comparison

### Creating a gRPC Client

#### C# (MAUI)
```csharp
// In MainPage.xaml.cs
using Grpc.Net.Client;
using GrpcServiceApi;

var channel = GrpcChannel.ForAddress("http://localhost:5195");
var client = new Greeter.GreeterClient(channel);

var request = new HelloRequest { Name = "World" };
var response = await client.SayHelloAsync(request);

Console.WriteLine(response.Message);
```

#### TypeScript (Next.js)
```typescript
// In src/lib/grpc-clients.ts
import * as grpc from '@grpc/grpc-js';
import * as protoLoader from '@grpc/proto-loader';

export class GreeterClient {
  private client: any;

  constructor(serverAddress: string = 'localhost:5195') {
    // Load proto file dynamically
    const packageDefinition = protoLoader.loadSync('greet.proto', {});
    const greetProto = grpc.loadPackageDefinition(packageDefinition);
    
    this.client = new greetProto.greet.Greeter(
      serverAddress,
      grpc.credentials.createInsecure()
    );
  }

  async sayHello(name: string): Promise<HelloReply> {
    return new Promise((resolve, reject) => {
      this.client.SayHello({ name }, (error, response) => {
        if (error) reject(error);
        else resolve(response);
      });
    });
  }
}

// Usage:
const client = new GreeterClient();
const response = await client.sayHello('World');
console.log(response.message);
```

---

### UI Code Comparison

#### C# XAML (MAUI)
```xml
<!-- MainPage.xaml -->
<ContentPage>
    <VerticalStackLayout>
        <Label Text="Enter your name:" />
        <Entry x:Name="NameEntry" Placeholder="Your name" />
        <Button Text="Say Hello" Clicked="OnSayHelloClicked" />
        <Label x:Name="ResultLabel" />
    </VerticalStackLayout>
</ContentPage>
```

```csharp
// MainPage.xaml.cs
private async void OnSayHelloClicked(object sender, EventArgs e)
{
    var name = NameEntry.Text;
    var client = new GreeterClient(channel);
    var response = await client.SayHelloAsync(new HelloRequest { Name = name });
    ResultLabel.Text = response.Message;
}
```

#### TypeScript/React (Next.js)
```typescript
// src/app/page.tsx
'use client';

import { useState } from 'react';

export default function Home() {
  const [name, setName] = useState('');
  const [result, setResult] = useState('');

  const handleGreet = async () => {
    // Call API route (which calls gRPC)
    const response = await fetch('/api/greet', {
      method: 'POST',
      body: JSON.stringify({ name }),
    });
    
    const data = await response.json();
    setResult(data.message);
  };

  return (
    <div>
      <label>Enter your name:</label>
      <input 
        value={name} 
        onChange={(e) => setName(e.target.value)} 
        placeholder="Your name"
      />
      <button onClick={handleGreet}>Say Hello</button>
      <p>{result}</p>
    </div>
  );
}
```

---

## 🌐 Why Next.js Needs API Routes

### The Problem:
**gRPC cannot run directly in a web browser!**

- gRPC requires HTTP/2 with specific features
- Browsers have limited HTTP/2 support for gRPC
- `@grpc/grpc-js` is a Node.js library, not browser-compatible

### The Solution: API Routes

```
┌─────────────────────────────────────────────────┐
│ Browser (Client-Side)                           │
│ - React Components (UI)                         │
│ - Makes HTTP requests to API routes             │
└────────────┬────────────────────────────────────┘
             │ fetch('/api/greet')
             ↓
┌─────────────────────────────────────────────────┐
│ Next.js Server (Server-Side)                    │
│ - API Routes (src/app/api/)                     │
│ - gRPC Clients run here                         │
│ - Calls gRPC services                           │
└────────────┬────────────────────────────────────┘
             │ gRPC call
             ↓
┌─────────────────────────────────────────────────┐
│ gRPC Server (.NET or NestJS)                    │
│ - Port 5194 (.NET) or 5195 (NestJS)            │
│ - Returns gRPC response                         │
└─────────────────────────────────────────────────┘
```

### In C# MAUI, you call gRPC directly:
```csharp
var response = await grpcClient.SayHelloAsync(request);
```

### In Next.js, you have two layers:
```typescript
// 1. Browser calls API route (HTTP)
const response = await fetch('/api/greet', {
  method: 'POST',
  body: JSON.stringify({ name: 'World' }),
});

// 2. API route calls gRPC (server-side)
// src/app/api/greet/route.ts
export async function POST(request) {
  const grpcClient = new GreeterClient();
  const grpcResponse = await grpcClient.sayHello(name);
  return NextResponse.json({ message: grpcResponse.message });
}
```

---

## 🎨 Styling Comparison

### C# MAUI
```xml
<Button 
    Text="Click Me" 
    BackgroundColor="Blue" 
    TextColor="White"
    HeightRequest="50"
    CornerRadius="10"
/>
```

### Next.js (Tailwind CSS)
```tsx
<button className="bg-blue-500 text-white h-12 rounded-lg">
  Click Me
</button>
```

---

## 📦 State Management

### C# MAUI (Fields and Properties)
```csharp
public partial class MainPage : ContentPage
{
    private string _name = "";
    private string _result = "";

    private void UpdateUI()
    {
        ResultLabel.Text = _result;
    }
}
```

### Next.js (React Hooks)
```typescript
export default function Home() {
  const [name, setName] = useState('');
  const [result, setResult] = useState('');

  // UI automatically updates when state changes
  // No manual UpdateUI() needed!
}
```

**Key Difference:**  
React automatically re-renders when state changes. MAUI requires manual UI updates.

---

## 🔄 Data Binding

### C# MAUI (Two-way binding)
```xml
<Entry Text="{Binding Name, Mode=TwoWay}" />
```

```csharp
public string Name
{
    get => _name;
    set { _name = value; OnPropertyChanged(); }
}
```

### Next.js (Controlled Components)
```tsx
const [name, setName] = useState('');

<input 
  value={name} 
  onChange={(e) => setName(e.target.value)} 
/>
```

---

## 🚀 Running the App

### C# MAUI
```bash
dotnet run
# Or in Rider: Click ▶️ Run button
# App launches as desktop/mobile app
```

### Next.js
```bash
npm run dev
# Or in Rider: Select "NextJSgRPC (Dev)" → Click ▶️
# Opens in browser: http://localhost:3000
```

---

## 📊 Feature Comparison

| Feature | C# MAUI | Next.js |
|---------|---------|---------|
| **Platform** | Desktop, Mobile | Web browser |
| **Hot Reload** | ✅ Yes | ✅ Yes (faster!) |
| **Debugging** | ✅ Breakpoints in Rider | ✅ Breakpoints in Rider |
| **gRPC** | Direct calls | Via API routes |
| **Offline** | ✅ Works offline | ❌ Needs internet |
| **Distribution** | App stores, installers | URL link |
| **Updates** | Reinstall app | Instant (reload page) |

---

## 🎓 Learning Path for .NET Developers

If you know C#/MAUI, here's what's different in Next.js:

### 1. **TypeScript = C# for JavaScript**
- Same type safety as C#
- Interfaces instead of classes (often)
- `async/await` works the same!

### 2. **React = Different UI paradigm**
- **MAUI:** Imperative (you tell it what to do)
  ```csharp
  ResultLabel.Text = "Hello";
  ```
  
- **React:** Declarative (you describe what you want)
  ```typescript
  <p>{result}</p>  // Automatically updates when `result` changes
  ```

### 3. **Components = Reusable UI pieces**
- Similar to UserControls in MAUI
- But more functional (less object-oriented)

### 4. **API Routes = Backend in Next.js**
- Think of them as mini ASP.NET Core controllers
- Handle server-side logic (like gRPC calls)

---

## 💡 Key Takeaways

### Same Concepts, Different Syntax:

| C# Concept | Next.js Equivalent |
|------------|-------------------|
| `class` | `function` or `class` |
| `async Task<T>` | `async Promise<T>` |
| `new Greeter.GreeterClient()` | `new GreeterClient()` |
| `await client.SayHelloAsync()` | `await client.sayHello()` |
| XAML markup | JSX/TSX |
| Event handlers | `onClick`, `onChange`, etc. |
| Properties | State hooks (`useState`) |
| Dependency Injection | Import/Export modules |

### Architecture:

**MAUI:**
```
App → gRPC Client → gRPC Server
```

**Next.js:**
```
Browser → API Route → gRPC Client → gRPC Server
```

---

## 📚 Quick Reference

### Running Commands:

| Task | C# | Next.js |
|------|----|---------| 
| **Install dependencies** | `dotnet restore` | `npm install` |
| **Run dev mode** | `dotnet run` | `npm run dev` |
| **Build** | `dotnet build` | `npm run build` |
| **Run production** | Publish app | `npm run start` |

### File Extensions:

| Type | C# | Next.js |
|------|----|---------| 
| **Code** | `.cs` | `.ts` (TypeScript) |
| **UI** | `.xaml` | `.tsx` (TypeScript + JSX) |
| **Config** | `.csproj`, `.json` | `.json`, `.js` |

---

## 🎉 You're Ready!

Now you understand:
- ✅ How Next.js compares to MAUI
- ✅ Why API routes are needed
- ✅ How to call gRPC from Next.js
- ✅ Key differences in syntax and architecture

**Next step:** Run the app and see it in action!

```bash
cd NextJSgRPC
npm install
npm run dev
# Open: http://localhost:3000
```

Happy coding! 🚀
