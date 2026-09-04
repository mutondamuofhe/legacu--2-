# Legacy Vault React client

This client mirrors the Legacy Vault mobile visual language with a responsive navy sidebar and Firebase-backed user data.

## Run

1. Install Node.js 20 or newer.
2. Copy `.env.example` to `.env.local`.
3. Fill in the Firebase Web App configuration values from Firebase Console.
4. Enable Email/Password Authentication and Firestore.
5. Run `npm install` then `npm run dev`.

Firestore data is read from `users/{uid}/assets`, `users/{uid}/documents`, `users/{uid}/executors`, and `users/{uid}/instructions`. Configure Firestore Security Rules so users can read and write only their own `users/{uid}` subtree. The admin UI activates for the exact account `admin@legacy.com`; production deployments should use a custom claim instead.
