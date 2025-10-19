# Personalized Learning Platform with AI Recommendation System

## 📘 Overview
The **Personalized Learning Platform** is an intelligent web-based system designed to recommend **career paths and learning tracks** based on users’ skills, interests, and goals.  
By integrating **AI-driven recommendations** with a user-friendly **ASP.NET MVC web interface**, this platform helps students and learners choose the most suitable educational track — such as **Web Development, Data Science, Artificial Intelligence, or Cyber Security** — and provides personalized **roadmaps, courses, and project ideas**.

---

## 🧠 Features

### 🔹 AI Recommendation System (Python)
- Analyzes user skills and goals using **Random Forest algorithm**  
- Suggests the best **career path** (Web Development, Data Science, Artificial Intelligence, or Cyber Security)  
- Ensures **reproducible results** using `random_state = 42`  

### 🔹 Web Platform (ASP.NET + SQL Server)
- **Login and Register pages** with secure authentication  
- **Dashboard** displaying AI recommendations and learning progress  
- **Admin Panel** for managing courses, tracks, and user feedback  
- **SQL Server** for structured data storage and quick retrieval  

### 🔹 Frontend (HTML, CSS, JavaScript)
- Interactive and responsive interface  
- Contains pages like: **Home**, **Login**, **Register**, **Dashboard**, and **Learning Track**  
- Clean and modern design for easy navigation  

---

## 🏗️ System Architecture

### **1. User Interface Layer (Frontend)**
- Built with **HTML, CSS, JavaScript**
- Provides interactive web pages for user input and displaying recommendations

### **2. Application Layer (ASP.NET Core)**
- Handles business logic and user authentication
- Implements **API Controllers** to connect frontend, AI model, and database

### **3. AI Recommendation Engine (Python)**
- Uses **Random Forest model** for career path prediction
- Processes user input through tokenization and vectorization
- Outputs learning track and suggested courses

### **4. Database Layer (SQL Server)**
- Manages user data, course content, and feedback logs

### **5. Admin Panel**
- Manage skill mappings, add/edit courses, and view user feedback

---

## ⚙️ Software Used

| Component | Technology | Purpose |
|------------|-------------|----------|
| Programming Language | C#, Python | Backend & AI Model |
| Framework | ASP.NET Core MVC | Web Application |
| Database | SQL Server | Data Storage |
| Frontend | HTML, CSS, JavaScript | User Interface |
| AI Libraries | Scikit-Learn, Pandas | Model Training & Prediction |

---

## 🧩 Installation & Setup

### 1️⃣ Clone the Repository
```bash
git clone https://github.com/Mohamed-Waliid/Personalized-Learning-Platform.git
```

### 2️⃣ Backend Setup (ASP.NET)
- Open the project in **Visual Studio**
- Update the `appsettings.json` connection string for SQL Server
- Run database migrations (if needed)
- Start the server using IIS Express or `dotnet run`

---

## 🧪 Testing
- Each module (Frontend, Backend, AI, Database) was **individually tested**
- Random Forest model performance achieved **95% accuracy**
- Manual and automated tests ensured correct data flow and recommendations

---

## 🚀 Future Enhancements
- Add **course progress tracking** and **certifications**
- Enable **chatbot assistance** using NLP
- Integrate **mobile application support**
