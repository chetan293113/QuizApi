// ✅ Check if JS is loaded
console.log("JS Loaded");

let answers = {};
let time = 60;
let timerInterval;

// ✅ Load Questions
async function loadQuestions() {
    console.log("Button clicked");

    try {
        // Start timer
        startTimer();

        const response = await fetch("/api/question");

        if (!response.ok) {
            throw new Error("Failed to fetch questions");
        }

        const data = await response.json();

        console.log("Questions:", data);

        let html = "";

        data.forEach(q => {
            html += `
                <div class="card p-3 mb-3">
                    <p><b>${q.questionText}</b></p>

                    <input type="radio" name="${q.id}" onclick="save(${q.id}, 'A')"> ${q.optionA}<br>
                    <input type="radio" name="${q.id}" onclick="save(${q.id}, 'B')"> ${q.optionB}<br>
                    <input type="radio" name="${q.id}" onclick="save(${q.id}, 'C')"> ${q.optionC}<br>
                    <input type="radio" name="${q.id}" onclick="save(${q.id}, 'D')"> ${q.optionD}<br>
                </div>
            `;
        });

        document.getElementById("quiz").innerHTML = html;

    } catch (error) {
        console.error("Error loading questions:", error);
        alert("Error loading questions");
    }
}

// ✅ Save answers
function save(questionId, answer) {
    answers[questionId] = answer;
}

// ✅ Submit Quiz
async function submitQuiz() {
    try {
        if (Object.keys(answers).length === 0) {
            alert("Please answer at least one question");
            return;
        }

        clearInterval(timerInterval);

        const userId = 1; // temporary

        const response = await fetch(`/api/quiz/submit?userId=${userId}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(answers)
        });

        if (!response.ok) {
            throw new Error("Submit failed");
        }

        const result = await response.json();

        document.getElementById("result").innerText =
            "Your Score: " + result.score;

    } catch (error) {
        console.error("Error submitting quiz:", error);
        alert("Error submitting quiz");
    }
}

// ✅ Timer Function
function startTimer() {
    time = 60;

    const timerElement = document.getElementById("timer");

    timerInterval = setInterval(() => {
        time--;

        if (timerElement) {
            timerElement.innerText = "Time: " + time;
        }

        if (time <= 0) {
            clearInterval(timerInterval);
            alert("Time's up!");
            submitQuiz();
        }
    }, 1000);
}