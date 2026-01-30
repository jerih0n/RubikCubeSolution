const colorMap = {
    0: 'transparent',
    1: '#FF6B35',
    2: '#FFFFFF',
    3: '#00A651',
    4: '#FFD700',
    5: '#DC143C',
    6: '#0066CC'
};

const colorNames = {
    0: 'None',
    1: 'Orange',
    2: 'White',
    3: 'Green',
    4: 'Yellow',
    5: 'Red',
    6: 'Blue'
};

let buttonPressHistory = [];

document.addEventListener('DOMContentLoaded', function() {
    initializeRotationButtons();
    initializeResetButton();
    initializeTestSequenceButton();
    updateHistoryDisplay();
});

function initializeRotationButtons() {
    const rotationButtons = document.querySelectorAll('.rotation-btn');
    
    rotationButtons.forEach(button => {
        button.addEventListener('click', async function() {
            const side = parseInt(this.getAttribute('data-side'));
            const clockwise = this.getAttribute('data-clockwise') === 'true';
            const label = this.getAttribute('data-label');
            addToHistory(label);
            await performRotation(side, clockwise);
        });
    });
}

function initializeResetButton() {
    const resetButton = document.getElementById('resetBtn');
    if (resetButton) {
        resetButton.addEventListener('click', function() {
            resetCube();
        });
    }
}

function initializeTestSequenceButton() {
    const testSequenceButton = document.getElementById('testSequenceBtn');
    if (testSequenceButton) {
        testSequenceButton.addEventListener('click', function() {
            runTestSequence();
        });
    }
}

function addToHistory(label) {
    buttonPressHistory.push(label);
    updateHistoryDisplay();
}

function updateHistoryDisplay() {
    const historyList = document.getElementById('historyList');
    
    if (!historyList) {
        return;
    }
    
    if (buttonPressHistory.length === 0) {
        historyList.innerHTML = '<p class="text-muted text-center">No rotations yet</p>';
    } else {
        let html = '';
        for (let i = 0; i < buttonPressHistory.length; i++) {
            html += `<div class="history-item">${buttonPressHistory[i]}</div>`;
        }
        historyList.innerHTML = html;
        historyList.scrollLeft = historyList.scrollWidth;
    }
}

async function performRotation(side, clockwise, showAlert = true) {
    const allButtons = document.querySelectorAll('.rotation-btn, .reset-btn, .test-sequence-btn');
    allButtons.forEach(btn => btn.disabled = true);

    try {
        const response = await fetch('/Home/Rotate', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ side: side, clockwise: clockwise })
        });

        if (!response.ok) {
            const err = await response.json();
            throw new Error(err.error || 'Network response was not ok');
        }

        const data = await response.json();
        
        if (data.matrix) {
            updateMatrix(data.matrix);
            return data;
        } else {
            throw new Error('Invalid response format');
        }
    } catch (error) {
        console.error('Error:', error);
        if (showAlert) {
            alert('Error performing rotation: ' + error.message);
        }
        throw error;
    } finally {
        allButtons.forEach(btn => btn.disabled = false);
    }
}

function updateMatrix(matrix) {
    const container = document.getElementById('matrixContainer');
    if (!container) {
        return;
    }
    
    container.innerHTML = '';

    for (let i = 0; i < matrix.length; i++) {
        const row = document.createElement('div');
        row.className = 'matrix-row';

        for (let j = 0; j < matrix[i].length; j++) {
            const cellValue = matrix[i][j];
            
            if (cellValue !== 0) {
                const cell = document.createElement('div');
                cell.className = 'matrix-cell';
                cell.style.backgroundColor = colorMap[cellValue];
                cell.title = getColorName(cellValue) + ' (' + i + ', ' + j + ')';
                row.appendChild(cell);
            } else {
                const emptyCell = document.createElement('div');
                emptyCell.className = 'matrix-cell-empty';
                row.appendChild(emptyCell);
            }
        }

        container.appendChild(row);
    }
}

function getColorName(value) {
    return colorNames[value] || 'Unknown';
}

function resetCube() {
    const buttons = document.querySelectorAll('.rotation-btn, .reset-btn, .test-sequence-btn');
    buttons.forEach(btn => btn.disabled = true);

    fetch('/Home/Reset', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        }
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.json();
    })
    .then(data => {
        updateMatrix(data.matrix);
        buttonPressHistory = [];
        updateHistoryDisplay();
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Error resetting cube: ' + error.message);
    })
    .finally(() => {
        const buttons = document.querySelectorAll('.rotation-btn, .reset-btn, .test-sequence-btn');
        buttons.forEach(btn => btn.disabled = false);
    });
}

async function runTestSequence() {
    const sequence = [
        { side: 3, clockwise: true, label: 'F' },
        { side: 5, clockwise: false, label: 'R\'' },
        { side: 2, clockwise: true, label: 'U' },
        { side: 6, clockwise: false, label: 'B\'' },
        { side: 1, clockwise: true, label: 'L' },
        { side: 4, clockwise: false, label: 'D\'' }
    ];

    try {
        for (let i = 0; i < sequence.length; i++) {
            const rotation = sequence[i];
            
            addToHistory(rotation.label);
            
            await performRotation(rotation.side, rotation.clockwise, false);
            
            if (i < sequence.length - 1) {
                await new Promise(resolve => setTimeout(resolve, 1000));
            }
        }
    } catch (error) {
        console.error('Error in test sequence:', error);
        alert('Error during test sequence: ' + error.message);
    }
}
