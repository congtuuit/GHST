import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

import "./App.css";
import Login from "./pages/login";
import ProtectedRoute from "./components/shared/ProtectedRoute";
import HomePage from "./pages/home";
import { AppProvider } from './context/AppContext';

function App() {
  return (
    <AppProvider>
      <div className="App">
        <Router>
          <Routes>
            <Route path="/login" element={<Login />} />
            <Route element={<ProtectedRoute />}>
              <Route path="/" element={<HomePage />} />
              {/* Add more protected routes here */}
            </Route>
          </Routes>
        </Router>
      </div>
    </AppProvider>
  );
}

export default App;