import React, { useState, useRef, useEffect } from "react";
import axios from "axios";
import "./App.css";
import bunnySticker from "./assets/bunny_sticker.png";
import textfightLogo from "./assets/textfight_logo.png";

function App() {
  const [subject, setSubject] = useState("");
  const [conversation, setConversation] = useState([]);
  const [loading, setLoading] = useState(false);
  const chatRef = useRef(null);

  const handleFight = async () => {
    if (!subject) return;
    setLoading(true);
    try {
      const res = await axios.post("http://localhost:5175/api/fight/battle", {
        subject,
      });

      const flatMessages = (res.data.conversation || []).map(msg => ({
        bot: msg.bot,
        text: msg.text
      }));

      setConversation(flatMessages);
      setSubject(""); // clear input after sending
    } catch (err) {
      console.error(err);
      alert("Error calling backend");
    } finally {
      setLoading(false);
    }
  };

  // Auto-scroll to bottom
  useEffect(() => {
    if (chatRef.current) {
      chatRef.current.scrollTop = chatRef.current.scrollHeight;
    }
  }, [conversation]);

  return (
    <div className="App">
  {/* Title on left */}
{/* Title on left with bunny + logo + description */}
<div className="title">
  <img src={bunnySticker} alt="Bunny Sticker" className="bunny" />
  <img src={textfightLogo} alt="Text Fight Logo" className="logo" />
  <p className="tagline">
    Watch two bots argue about anything you throw at them 🥊  
    Silly debates, epic roasts, and lots of nonsense guaranteed!
  </p>
</div>




  {/* Phone wrapper on right */}
  <div className="phone-wrapper">
    <div className="right-panel" ref={chatRef}>
      <div className="conversation">
        {conversation.map((msg, idx) => (
          <div key={idx} className={`bubble ${msg.bot === "Bot1" ? "bot1" : "bot2"}`}>
            <strong>{msg.bot}:</strong> {msg.text || "NO TEXT"}
          </div>
        ))}
      </div>
      <div className="input-area">
        <input
          type="text"
          placeholder="Enter a topic..."
          value={subject}
          onChange={(e) => setSubject(e.target.value)}
        />
        <button onClick={handleFight} disabled={loading}>
          {loading ? "Fighting..." : "Send"}
        </button>
      </div>
    </div>
  </div>
</div>

  );
}

export default App;
