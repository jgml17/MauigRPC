'use client';

/**
 * Main Page - Similar to MainPage.xaml in MAUI
 * 
 * In MAUI, you have:
 * - MainPage.xaml (UI markup)
 * - MainPage.xaml.cs (code-behind)
 * 
 * In Next.js, you have:
 * - page.tsx (combines UI and logic in one file using React)
 */

import { useState } from 'react';
import Link from 'next/link';

export default function Home() {
  // State (similar to private fields in C#)
  const [name, setName] = useState('');
  const [result, setResult] = useState('');
  const [loading, setLoading] = useState(false);
  const [server, setServer] = useState<'dotnet' | 'nestjs'>('nestjs');

  // Event handler (similar to OnClicked in MAUI)
  const handleGreet = async () => {
    setLoading(true);
    try {
      // Call API route (which then calls gRPC)
      const response = await fetch('/api/greet', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name, server }),
      });
      
      const data = await response.json();
      
      if (data.success) {
        setResult(`${data.message} (from ${data.server} server)`);
      } else {
        setResult(`Error: ${data.error}`);
      }
    } catch (error: any) {
      setResult(`Error: ${error.message}`);
    } finally {
      setLoading(false);
    }
  };

  // Render (similar to XAML markup)
  return (
    <main className="min-h-screen p-8 bg-gray-50">
      <div className="max-w-4xl mx-auto">
        {/* Title */}
        <h1 className="text-4xl font-bold mb-8 text-center">
          Next.js gRPC Client
        </h1>
        
        <div className="bg-white rounded-lg shadow-md p-6 mb-6">
          <h2 className="text-2xl font-semibold mb-4">Greeter Service</h2>
          
          {/* Server Selection (like Picker in MAUI) */}
          <div className="mb-4">
            <label className="block mb-2 font-medium">Server:</label>
            <select 
              value={server}
              onChange={(e) => setServer(e.target.value as 'dotnet' | 'nestjs')}
              className="w-full p-2 border rounded"
            >
              <option value="nestjs">NestJS (Port 5195)</option>
              <option value="dotnet">.NET (Port 5194)</option>
            </select>
          </div>

          {/* Name Input (like Entry in MAUI) */}
          <div className="mb-4">
            <label className="block mb-2 font-medium">Your Name:</label>
            <input
              type="text"
              value={name}
              onChange={(e) => setName(e.target.value)}
              placeholder="Enter your name"
              className="w-full p-2 border rounded"
              onKeyPress={(e) => e.key === 'Enter' && handleGreet()}
            />
          </div>

          {/* Button (like Button in MAUI) */}
          <button
            onClick={handleGreet}
            disabled={loading || !name}
            className="w-full bg-blue-500 text-white p-3 rounded hover:bg-blue-600 disabled:bg-gray-400"
          >
            {loading ? 'Calling gRPC...' : 'Say Hello'}
          </button>

          {/* Result Label (like Label in MAUI) */}
          {result && (
            <div className="mt-4 p-4 bg-green-50 border border-green-200 rounded">
              <p className="font-medium">{result}</p>
            </div>
          )}
        </div>

        {/* Navigation to Contacts */}
        <div className="text-center">
          <Link 
            href="/contacts"
            className="inline-block bg-gray-700 text-white px-6 py-3 rounded hover:bg-gray-800"
          >
            View Contacts (CRUD Operations)
          </Link>
        </div>

        {/* Info Panel */}
        <div className="mt-8 bg-blue-50 border border-blue-200 rounded-lg p-6">
          <h3 className="font-semibold mb-2">🎯 How It Works:</h3>
          <ul className="list-disc list-inside space-y-1 text-sm">
            <li>Browser → Next.js API Route → gRPC Client → gRPC Server</li>
            <li>Similar to MAUI app calling gRPC directly</li>
            <li>Can switch between .NET (5194) and NestJS (5195) servers</li>
          </ul>
        </div>
      </div>
    </main>
  );
}
