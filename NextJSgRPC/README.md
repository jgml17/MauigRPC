# Next.js gRPC Client

A web-based gRPC client built with Next.js, similar to your MAUI app but for the browser.

---

## 🚀 Quick Start

### 1. Install Dependencies
```bash
cd NextJSgRPC
npm install
```

### 2. Run Development Server
```bash
npm run dev
```

### 3. Open Browser
Navigate to: http://localhost:3005

---

## 📊 Features

✅ Call both **.NET (5194)** and **NestJS (5195)** gRPC services  
✅ Greeter service integration  
✅ Contact CRUD operations  
✅ Switch between servers dynamically  
✅ Modern UI with Tailwind CSS  
✅ TypeScript for type safety  

---

## 🏗️ Architecture

```
Browser (React UI)
    ↓ HTTP Request
Next.js API Routes (Server-side)
    ↓ gRPC Call
.NET gRPC Service (5194) or NestJS gRPC Service (5195)
```

**Why API Routes?** gRPC cannot run directly in browser, so Next.js API routes act as a proxy.

---

## 📁 Project Structure

```
NextJSgRPC/
├── src/
│   ├── app/
│   │   ├── page.tsx              # Home page
│   │   ├── layout.tsx            # App layout
│   │   └── api/                  # Server-side API routes
│   │       ├── greet/route.ts    # Greeter service proxy
│   │       └── contacts/route.ts # Contact service proxy
│   ├── lib/
│   │   └── grpc-clients.ts       # gRPC client library
│   └── components/               # React components
├── proto/                        # Proto files (copied from .NET project)
├── package.json                  # Dependencies
└── next.config.js               # Next.js configuration
```

---

## 🎯 Available Scripts

```bash
npm run dev      # Development mode (port 3005)
npm run build    # Build for production
npm run start    # Run production build (port 3005)
npm run lint     # Run linter
```

---

## 🔧 Running in Rider

After restarting Rider, you'll see these configurations:

- **NextJSgRPC (Dev)** - Development mode ⭐
- **NextJSgRPC (Build)** - Build production
- **NextJSgRPC (Production)** - Run production build

Just select and click ▶️ Run!

---

## 📚 Documentation

### For .NET Developers New to Next.js:

- **NEXTJS_VS_DOTNET.md** - Complete framework comparison (MAUI vs Next.js)
- **docs/CONTACT_DATA_EXPLAINED.md** - Deep dive into contact data structure and naming conventions ⭐
- **README.md** - This file (quick start)

### Why Read the Documentation?

If you're coming from C#/.NET and wondering:
- 🤔 Why phone numbers use `phoneNumbers` instead of `PhoneNumbers`?
- 🤔 How does data flow from gRPC to the UI?
- 🤔 What's the difference between snake_case, camelCase, and PascalCase?
- 🤔 How does this compare to my MAUI app?

**Read `docs/CONTACT_DATA_EXPLAINED.md`** - It explains everything in .NET terms!

---

## 🧪 Testing

### Test Greeter Service:
1. Open http://localhost:3005
2. Enter your name
3. Select server (.NET or NestJS)
4. Click "Say Hello"

### Test Contact Service:
1. Navigate to http://localhost:3005/contacts
2. View all contacts with complete information:
   - Name and ID
   - Full address (street, city, state, zip code, country)
   - Multiple phone numbers with types (Mobile, Home, Work)
3. Add new contacts with full details
4. Edit existing contacts
5. Delete contacts

---

## 🌐 API Routes

### POST /api/greet
Call the Greeter service

**Request:**
```json
{
  "name": "World",
  "server": "nestjs"  // or "dotnet"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Hello World",
  "server": "nestjs"
}
```

### GET /api/contacts
Get all contacts

**Query params:** `?server=nestjs` or `?server=dotnet`

### POST /api/contacts
Create new contact

---

## 🎨 Tech Stack

- **Next.js 14** - React framework
- **TypeScript** - Type safety
- **Tailwind CSS** - Styling
- **@grpc/grpc-js** - gRPC client
- **React** - UI library

---

## 🔄 Comparison with MAUI

| Feature | MAUI | Next.js |
|---------|------|---------|
| Platform | Desktop/Mobile | Web Browser |
| UI | XAML | React/JSX |
| gRPC | Direct | Via API Routes |
| Language | C# | TypeScript |
| Hot Reload | ✅ | ✅ |
| Debugging | ✅ | ✅ |

See **NEXTJS_VS_DOTNET.md** for detailed comparison.

---

## 🐛 Troubleshooting

### Port 3005 already in use
```bash
lsof -ti:3005 | xargs kill -9
```

### gRPC connection errors
Make sure the gRPC services are running:
- .NET: http://localhost:5194
- NestJS: http://localhost:5195

### Module not found errors
```bash
npm install
```

---

## 🎉 Next Steps

### Quick Start:
1. ✅ Run `npm install`
2. ✅ Run `npm run dev`
3. ✅ Open http://localhost:3005
4. ✅ Test the Greeter service
5. ✅ Navigate to /contacts and test CRUD operations

### Learn More (Highly Recommended for .NET Developers):
6. 📖 Read **docs/CONTACT_DATA_EXPLAINED.md** to understand data flow
7. 📖 Read **NEXTJS_VS_DOTNET.md** for framework comparison
8. 🎯 Compare the code with your MAUI app

### What Makes This Different from Your MAUI App:
- **Browser-based** instead of desktop/mobile
- **API routes** as middleware (browsers can't call gRPC directly)
- **camelCase** naming (JavaScript convention) vs **PascalCase** (C# convention)
- **React hooks** for state vs C# properties
- **Same gRPC backend** - works with both your .NET and NestJS services!

Happy coding! 🚀
