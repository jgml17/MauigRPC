# Quick Reference for .NET Developers

A cheat sheet for common tasks when moving from C#/.NET to Next.js/TypeScript.

---

## 🚀 Getting Started

### First Time Setup
```bash
cd NextJSgRPC
npm install          # Like: dotnet restore
npm run dev          # Like: dotnet run
```

### Daily Development
```bash
npm run dev          # Start dev server (auto-reload on changes)
# Open: http://localhost:3005
```

---

## 📂 File Organization

### Where to Find Things

| What You Want | C# MAUI | Next.js |
|---------------|---------|---------|
| **UI Page** | `MainPage.xaml` | `src/app/page.tsx` |
| **Code-behind** | `MainPage.xaml.cs` | Same file as UI |
| **API/Server logic** | Separate ASP.NET project | `src/app/api/*/route.ts` |
| **Data models** | `Models/*.cs` | Interfaces in each file |
| **gRPC client** | Auto-generated | `src/lib/grpc-clients.ts` |
| **Proto files** | `Protos/*.proto` | `proto/*.proto` |
| **Dependencies** | `.csproj` | `package.json` |
| **Config** | `appsettings.json` | `next.config.js` |

---

## 💻 Common Code Patterns

### 1. Declaring Variables

```csharp
// C#
private string name = "";
private int count = 0;
private Contact? contact = null;
```

```typescript
// TypeScript
const [name, setName] = useState('');
const [count, setCount] = useState(0);
const [contact, setContact] = useState<Contact | null>(null);
```

### 2. Updating State

```csharp
// C#
name = "John";
count = 5;
ResultLabel.Text = result;  // Manual UI update
```

```typescript
// TypeScript
setName('John');
setCount(5);
// UI updates automatically!
```

### 3. Event Handlers

```csharp
// C# - XAML
<Button Text="Click Me" Clicked="OnButtonClicked" />

// C# - Code-behind
private void OnButtonClicked(object sender, EventArgs e)
{
    // Handle click
}
```

```typescript
// TypeScript - JSX (all in one file)
<button onClick={handleClick}>
  Click Me
</button>

const handleClick = () => {
  // Handle click
};
```

### 4. Async/Await

```csharp
// C# - Almost the same!
private async Task<Contact> GetContactAsync(int id)
{
    var response = await client.GetContactAsync(new GetContactRequest { Id = id });
    return response.Contact;
}
```

```typescript
// TypeScript - Very similar!
const getContact = async (id: number): Promise<Contact> => {
  const response = await fetch(`/api/contacts?id=${id}`);
  const data = await response.json();
  return data.contact;
};
```

### 5. Lists/Arrays

```csharp
// C#
List<Contact> contacts = new List<Contact>();
contacts.Add(newContact);
var firstContact = contacts[0];
var names = contacts.Select(c => c.Name).ToList();
```

```typescript
// TypeScript
const contacts: Contact[] = [];
contacts.push(newContact);
const firstContact = contacts[0];
const names = contacts.map(c => c.name);
```

### 6. Conditional Rendering

```xml
<!-- C# XAML -->
<Label Text="No contacts" IsVisible="{Binding HasNoContacts}" />
<CollectionView IsVisible="{Binding HasContacts}" ... />
```

```typescript
// TypeScript JSX
{contacts.length === 0 ? (
  <p>No contacts</p>
) : (
  <div>{/* Show contacts */}</div>
)}
```

### 7. Looping Through Items

```xml
<!-- C# XAML -->
<CollectionView ItemsSource="{Binding Contacts}">
  <CollectionView.ItemTemplate>
    <DataTemplate>
      <Label Text="{Binding Name}" />
    </DataTemplate>
  </CollectionView.ItemTemplate>
</CollectionView>
```

```typescript
// TypeScript JSX
{contacts.map((contact) => (
  <div key={contact.id}>
    <label>{contact.name}</label>
  </div>
))}
```

---

## 🌐 Making API Calls

### In C# MAUI (Direct gRPC)

```csharp
// Direct to gRPC server
var channel = GrpcChannel.ForAddress("http://localhost:5195");
var client = new ContactService.ContactServiceClient(channel);

// GET
var response = await client.GetAllContactsAsync(new GetAllContactsRequest());
var contacts = response.Contacts;

// CREATE
var newContact = new CreateContactRequest 
{ 
    Name = "John",
    Address = new Address { City = "Portland" },
    PhoneNumbers = { new PhoneNumber { Number = "555-1234" } }
};
var createResponse = await client.CreateContactAsync(newContact);
```

### In Next.js (Via API Route)

```typescript
// From React component - call API route (HTTP)
// GET
const response = await fetch('/api/contacts');
const data = await response.json();
const contacts = data.contacts;

// CREATE
const newContact = {
  name: 'John',
  address: { city: 'Portland' },
  phoneNumbers: [{ number: '555-1234', type: 'MOBILE' }],
};

const response = await fetch('/api/contacts', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify(newContact),
});
const data = await response.json();
```

**Why different?** Browsers can't call gRPC directly. The API route (`src/app/api/contacts/route.ts`) acts as a proxy.

---

## 🎨 Styling

### C# MAUI
```xml
<Button 
    Text="Click Me"
    BackgroundColor="Blue"
    TextColor="White"
    HeightRequest="50"
    WidthRequest="200"
    CornerRadius="10"
/>
```

### Next.js (Tailwind CSS)
```typescript
<button className="bg-blue-500 text-white h-12 w-48 rounded-lg">
  Click Me
</button>
```

### Common Tailwind Classes

| MAUI Property | Tailwind Class | Example |
|---------------|----------------|---------|
| `BackgroundColor="Blue"` | `bg-blue-500` | `className="bg-blue-500"` |
| `TextColor="White"` | `text-white` | `className="text-white"` |
| `FontSize="Large"` | `text-lg` or `text-xl` | `className="text-lg"` |
| `FontAttributes="Bold"` | `font-bold` | `className="font-bold"` |
| `HeightRequest="50"` | `h-12` (48px) | `className="h-12"` |
| `WidthRequest="200"` | `w-48` (192px) | `className="w-48"` |
| `CornerRadius="10"` | `rounded-lg` | `className="rounded-lg"` |
| `Padding="20"` | `p-5` | `className="p-5"` |
| `Margin="10"` | `m-2.5` | `className="m-2.5"` |

---

## 🔧 TypeScript Quick Tips

### Type Annotations (Like C# Types)

```csharp
// C#
private string name;
private int age;
private Contact? contact;
private List<string> names;
```

```typescript
// TypeScript
const name: string;
const age: number;
const contact: Contact | null;
const names: string[];
```

### Interfaces (Like C# Classes/Interfaces)

```csharp
// C#
public interface IContact
{
    int Id { get; set; }
    string Name { get; set; }
    Address? Address { get; set; }
}
```

```typescript
// TypeScript
interface Contact {
  id: number;
  name: string;
  address?: Address;  // ? means optional (nullable)
}
```

### Optional Parameters

```csharp
// C#
public void Greet(string name = "World")
{
    Console.WriteLine($"Hello {name}");
}
```

```typescript
// TypeScript
const greet = (name: string = 'World') => {
  console.log(`Hello ${name}`);
};
```

---

## 📝 Common Tasks

### Task: Add a New Page

#### C# MAUI
1. Add `NewPage.xaml`
2. Add `NewPage.xaml.cs`
3. Navigate: `await Shell.Current.GoToAsync("NewPage");`

#### Next.js
1. Create `src/app/newpage/page.tsx`
2. Navigate: `<Link href="/newpage">Go</Link>` or `router.push('/newpage')`

### Task: Call gRPC Service

#### C# MAUI
```csharp
var channel = GrpcChannel.ForAddress("http://localhost:5195");
var client = new Greeter.GreeterClient(channel);
var response = await client.SayHelloAsync(new HelloRequest { Name = "World" });
```

#### Next.js (in API route)
```typescript
// src/app/api/greet/route.ts
import { GreeterClient } from '@/lib/grpc-clients';

export async function POST(request: NextRequest) {
  const { name } = await request.json();
  const client = new GreeterClient('localhost:5195');
  const response = await client.sayHello(name);
  return NextResponse.json({ message: response.message });
}
```

#### Next.js (from React component)
```typescript
// Call the API route
const response = await fetch('/api/greet', {
  method: 'POST',
  body: JSON.stringify({ name: 'World' }),
});
const data = await response.json();
console.log(data.message);
```

### Task: Show/Hide Elements

#### C# MAUI
```xml
<Label Text="Loading..." IsVisible="{Binding IsLoading}" />
<Label Text="Done!" IsVisible="{Binding IsNotLoading}" />
```

#### Next.js
```typescript
{loading && <p>Loading...</p>}
{!loading && <p>Done!</p>}
```

### Task: Handle Form Input

#### C# MAUI
```xml
<Entry Text="{Binding Name, Mode=TwoWay}" />
```

```csharp
private string _name;
public string Name
{
    get => _name;
    set { _name = value; OnPropertyChanged(); }
}
```

#### Next.js
```typescript
const [name, setName] = useState('');

<input 
  value={name} 
  onChange={(e) => setName(e.target.value)} 
/>
```

---

## 🐛 Debugging

### C# MAUI in Rider
1. Set breakpoint in `.cs` file
2. Click Debug ▶️
3. Breakpoint hits

### Next.js in Rider
1. Run `npm run dev` in terminal
2. Or use Rider run configuration "NextJSgRPC (Dev)"
3. Open browser DevTools (F12)
4. Use `console.log()` for debugging
5. Or attach Rider debugger to Node.js process

### Console Logging

```csharp
// C#
Console.WriteLine($"Name: {name}");
Debug.WriteLine($"Contact: {contact.Name}");
```

```typescript
// TypeScript
console.log(`Name: ${name}`);
console.log('Contact:', contact);  // Shows object structure
console.table(contacts);  // Nice table view for arrays
```

---

## ⚡ Hot Tips

### 1. TypeScript is Your Friend
If you see red squiggles in VS Code/Rider, **don't ignore them!** They're like C# compile errors.

### 2. Use Console.log Liberally
```typescript
console.log('Data received:', data);
console.log('Contact:', contact);
```

### 3. Check Browser Console
- Press F12 in browser
- Check Console tab for errors and logs
- Check Network tab to see API calls

### 4. Auto-Reload is Fast
- Save file → browser auto-reloads
- Much faster than C# compile + run!

### 5. TypeScript Errors in Terminal
Watch the terminal running `npm run dev` for compile errors.

---

## 🆚 Key Differences Summary

| Concept | C# MAUI | Next.js |
|---------|---------|---------|
| **UI + Logic** | Separate files (.xaml + .cs) | Same file (.tsx) |
| **State Updates** | Manual (`ResultLabel.Text = x`) | Automatic (React re-renders) |
| **gRPC Calls** | Direct from app | Via API route proxy |
| **Styling** | XAML properties | CSS/Tailwind classes |
| **Navigation** | Shell navigation | Links/Router |
| **Async** | `async Task<T>` | `async Promise<T>` |
| **Loops** | `CollectionView` | `.map()` |
| **Conditionals** | `IsVisible` binding | `{condition && <element>}` |
| **Naming** | PascalCase | camelCase |

---

## 🎓 Learning Resources

### If You Want to Dive Deeper:

1. **TypeScript for C# Developers**
   - Very similar to C#
   - Strong typing, interfaces, async/await
   - Main difference: Functions over classes

2. **React Basics**
   - Components = Reusable UI pieces
   - State = Data that can change
   - Props = Parameters passed to components
   - Hooks = Special functions (`useState`, `useEffect`)

3. **Next.js Concepts**
   - App Router = File-based routing
   - API Routes = Backend endpoints
   - Server/Client Components = Where code runs

---

## 🚀 Ready to Code!

### Quick Commands
```bash
npm run dev          # Start development server
npm run build        # Build for production  
npm run start        # Run production build
npm install          # Install dependencies
```

### Quick Navigation
- Home: http://localhost:3005
- Contacts: http://localhost:3005/contacts
- Add Contact: http://localhost:3005/contacts/add

### Need Help?
- Check `NEXTJS_VS_DOTNET.md` for detailed comparison
- Check `docs/CONTACT_DATA_EXPLAINED.md` for data flow explanation
- Look at the code - it's commented!

Happy coding! 🎉
